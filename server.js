const jsonServer = require('json-server');
const { v4: uuid } = require('uuid');

const server = jsonServer.create();
const router = jsonServer.router('db.json');
const middlewares = jsonServer.defaults();

server.use(middlewares);
server.use(jsonServer.bodyParser);

// ---- Helper: wrap list with { data, meta } ----
function withPaging(data, req) {
  const page = parseInt(req.query.page || req.query.pageNumber || '1', 10);
  const pageSize = parseInt(req.query.pageSize || '50', 10);

  const totalCount = data.length;
  const totalPages = Math.max(1, Math.ceil(totalCount / pageSize));
  const start = (page - 1) * pageSize;
  const end = start + pageSize;

  return {
    data: data.slice(start, end),
    meta: {
      currentPage: page,
      pageSize,
      totalCount,
      totalPages,
      hasPrevious: page > 1,
      hasNext: page < totalPages
    }
  };
}

// ---- Helper: fee calculation (like TariffCalculator) ----
function calcFee(tariff, entryTimeStr, exitTimeStr) {
  const entry = new Date(entryTimeStr);
  const exit = new Date(exitTimeStr);
  if (exit <= entry) return 0;

  const totalMinutes = (exit - entry) / 60000; // ms -> minutes
  if (totalMinutes <= tariff.gracePeriodMinutes) return 0;

  const billableMinutes = totalMinutes - tariff.gracePeriodMinutes;
  const periods = Math.ceil(billableMinutes / tariff.billingPeriodMinutes);
  let amount = periods * tariff.ratePerHour;

  if (tariff.dailyMaximum != null) {
    const days = (exit.toDateString() === entry.toDateString())
      ? 1
      : Math.ceil((exit - entry) / (24 * 60 * 60 * 1000));
    const cap = days * tariff.dailyMaximum;
    if (amount > cap) amount = cap;
  }

  return amount;
}

// ---- Small middleware: strip /api prefix so Angular can call /api/... ----
server.use((req, res, next) => {
  if (req.url.startsWith('/api/')) {
    req.url = req.url.substring(4); // remove "/api"
  } else if (req.url === '/api') {
    req.url = '/';
  }
  next();
});

// ---- HEALTH ----
server.get('/health/live', (req, res) => {
  res.json({ status: 'Healthy' });
});

// ====== PARKING LOTS ======

// GET /api/ParkingLot  -> { data, meta }
server.get('/ParkingLot', (req, res) => {
  const db = router.db;
  const lots = db.get('parkingLots').value();
  res.json(withPaging(lots, req));
});

// GET /api/ParkingLot/:id
server.get('/ParkingLot/:id', (req, res) => {
  const db = router.db;
  const lot = db.get('parkingLots').find({ id: req.params.id }).value();
  if (!lot) return res.status(404).json({ code: 'not_found', message: 'Lot not found' });
  res.json({ data: lot });
});

// GET /api/ParkingLot/:id/occupancy  -> simple metrics
server.get('/ParkingLot/:id/occupancy', (req, res) => {
  const db = router.db;
  const spots = db.get('parkingSpots').filter({ lotId: req.params.id }).value();
  const tickets = db.get('tickets').filter({ lotId: req.params.id }).value();

  const totalSpots = spots.length;
  const freeSpots = spots.filter(s => s.status === 0).length;
  const occupiedSpots = spots.filter(s => s.status === 1).length;
  const unavailableSpots = spots.filter(s => s.status === 2).length;
  const openTickets = tickets.filter(t => t.status === 0).length;

  res.json({
    data: {
      lotId: req.params.id,
      totalSpots,
      freeSpots,
      occupiedSpots,
      unavailableSpots,
      openTickets
    }
  });
});

// ====== PARKING SPOTS ======

// GET /api/lots/:lotId/ParkingSpot
server.get('/lots/:lotId/ParkingSpot', (req, res) => {
  const db = router.db;
  let spots = db.get('parkingSpots').filter({ lotId: req.params.lotId }).value();

  // optional filtering by status/type/code via query params
  if (req.query.status != null) {
    const status = Number(req.query.status);
    spots = spots.filter(s => s.status === status);
  }
  if (req.query.type != null) {
    const type = Number(req.query.type);
    spots = spots.filter(s => s.type === type);
  }
  if (req.query.code) {
    const code = req.query.code.toLowerCase();
    spots = spots.filter(s => s.spotCode.toLowerCase().includes(code));
  }

  res.json(withPaging(spots, req));
});

// PATCH /api/lots/:lotId/ParkingSpot/:spotId/status
server.patch('/lots/:lotId/ParkingSpot/:spotId/status', (req, res) => {
  const db = router.db;
  const { status } = req.body; // expect number 0/1/2
  let spot = db.get('parkingSpots').find({ id: req.params.spotId, lotId: req.params.lotId }).value();

  if (!spot) {
    return res.status(404).json({ code: 'spot_not_found', message: 'Spot not found' });
  }

  spot = { ...spot, status: Number(status) };
  db.get('parkingSpots').find({ id: spot.id }).assign(spot).write();

  res.json({ data: spot });
});

// ====== TARIFFS ======

// GET /api/lots/:lotId/Tariff/current
server.get('/lots/:lotId/Tariff/current', (req, res) => {
  const db = router.db;
  const tariffs = db.get('tariffs').filter({ lotId: req.params.lotId }).value();
  if (!tariffs.length) {
    return res.status(404).json({ code: 'tariff_not_found', message: 'No tariff for lot' });
  }
  const current = tariffs.sort((a, b) => new Date(b.effectiveFrom) - new Date(a.effectiveFrom))[0];
  res.json(current);
});

// GET /api/lots/:lotId/Tariff/history
server.get('/lots/:lotId/Tariff/history', (req, res) => {
  const db = router.db;
  const tariffs = db.get('tariffs').filter({ lotId: req.params.lotId }).value()
    .sort((a, b) => new Date(b.effectiveFrom) - new Date(a.effectiveFrom));
  res.json(tariffs);
});

// POST /api/lots/:lotId/Tariff
server.post('/lots/:lotId/Tariff', (req, res) => {
  const db = router.db;
  const body = req.body;
  const newTariff = {
    id: uuid(),
    lotId: req.params.lotId,
    ratePerHour: body.ratePerHour,
    billingPeriodMinutes: body.billingPeriodMinutes,
    gracePeriodMinutes: body.gracePeriodMinutes,
    dailyMaximum: body.dailyMaximum ?? null,
    lostTicketFee: body.lostTicketFee ?? null,
    effectiveFrom: body.effectiveFrom || new Date().toISOString()
  };
  db.get('tariffs').push(newTariff).write();
  res.status(201).json(newTariff);
});

// ====== TICKETS ======

// POST /api/tickets/enter
server.post('/tickets/enter', (req, res) => {
  const db = router.db;
  const { lotId, spotId, plateNumber } = req.body;

  const lot = db.get('parkingLots').find({ id: lotId }).value();
  if (!lot) return res.status(400).json({ code: 'lot_not_found', message: 'Lot not found' });

  const spot = db.get('parkingSpots').find({ id: spotId, lotId }).value();
  if (!spot) return res.status(400).json({ code: 'spot_not_found', message: 'Spot not found' });
  if (spot.status !== 0) {
    return res.status(409).json({ code: 'spot_not_available', message: 'Spot not free' });
  }

  const now = new Date().toISOString();

  const newTicket = {
    id: uuid(),
    lotId,
    spotId,
    spotCode: spot.spotCode,
    vehicle: {
      plate: plateNumber,
      type: 0,
      color: null
    },
    entryTime: now,
    exitTime: null,
    status: 0, // Open
    computedAmount: null,
    isPaid: false,
    createdAt: now,
    updatedAt: now
  };

  // mark spot occupied
  db.get('parkingSpots').find({ id: spotId }).assign({ status: 1 }).write();
  db.get('tickets').push(newTicket).write();

  res.status(201).json(newTicket);
});

// GET /api/tickets/:ticketId/preview-exit
server.get('/tickets/:ticketId/preview-exit', (req, res) => {
  const db = router.db;
  const ticket = db.get('tickets').find({ id: req.params.ticketId }).value();
  if (!ticket) {
    return res.status(404).json({ code: 'ticket_not_found', message: 'Ticket not found' });
  }

  const tariffs = db.get('tariffs').filter({ lotId: ticket.lotId }).value();
  if (!tariffs.length) {
    return res.status(400).json({ code: 'tariff_not_found', message: 'No tariff for lot' });
  }
  const tariff = tariffs.sort((a, b) => new Date(b.effectiveFrom) - new Date(a.effectiveFrom))[0];

  const now = new Date().toISOString();
  const amount = calcFee(tariff, ticket.entryTime, now);

  const entry = new Date(ticket.entryTime);
  const current = new Date(now);
  const totalMinutes = Math.max(0, (current - entry) / 60000);
  const overGrace = Math.max(0, totalMinutes - tariff.gracePeriodMinutes);
  const billingPeriods = overGrace <= 0 ? 0 : Math.ceil(overGrace / tariff.billingPeriodMinutes);
  const billableMinutes = billingPeriods * tariff.billingPeriodMinutes;

  const preview = {
    ticketId: ticket.id,
    entryTime: ticket.entryTime,
    currentTime: now,
    durationMinutes: Math.round(totalMinutes),
    gracePeriodMinutes: tariff.gracePeriodMinutes,
    billableMinutes,
    billingPeriods,
    ratePerHour: tariff.ratePerHour,
    computedFee: amount,
    dailyMaximum: tariff.dailyMaximum ?? undefined,
    appliedFee: amount
  };

  res.json(preview);
});

// POST /api/tickets/close
server.post('/tickets/close', (req, res) => {
  const db = router.db;
  const { ticketId, paymentMethod, isLostTicket } = req.body;

  let ticket = db.get('tickets').find({ id: ticketId }).value();
  if (!ticket) return res.status(404).json({ code: 'ticket_not_found', message: 'Ticket not found' });
  if (ticket.status === 1) {
    return res.status(409).json({ code: 'ticket_already_closed', message: 'Ticket already closed' });
  }

  const tariffs = db.get('tariffs').filter({ lotId: ticket.lotId }).value();
  if (!tariffs.length) {
    return res.status(400).json({ code: 'tariff_not_found', message: 'No tariff for lot' });
  }
  const tariff = tariffs.sort((a, b) => new Date(b.effectiveFrom) - new Date(a.effectiveFrom))[0];

  const now = new Date().toISOString();
  let amount;

  if (isLostTicket && tariff.lostTicketFee != null) {
    amount = tariff.lostTicketFee;
  } else {
    amount = calcFee(tariff, ticket.entryTime, now);
  }

  ticket = {
    ...ticket,
    exitTime: now,
    status: 1, // Closed
    computedAmount: amount,
    isPaid: true,
    updatedAt: now
  };

  db.get('tickets').find({ id: ticketId }).assign(ticket).write();

  const payment = {
    id: uuid(),
    ticketId,
    amount,
    method: Number(paymentMethod),
    paidAt: now,
    reference: `PAY-${ticketId}`
  };
  db.get('payments').push(payment).write();

  // free spot
  db.get('parkingSpots').find({ id: ticket.spotId }).assign({ status: 0 }).write();

  res.json(ticket);
});

// POST /api/tickets/search
server.post('/tickets/search', (req, res) => {
  const db = router.db;
  let tickets = db.get('tickets').value();

  const { status, plate, lotId, spotId, from, to } = req.body || {};

  if (status !== undefined && status !== null) {
    const statusNum = typeof status === 'string' && isNaN(Number(status))
      ? (status === 'Open' ? 0 : status === 'Closed' ? 1 : 2)
      : Number(status);
    tickets = tickets.filter(t => t.status === statusNum);
  }

  if (plate) {
    const lower = plate.toLowerCase();
    tickets = tickets.filter(t => t.vehicle && t.vehicle.plate.toLowerCase().includes(lower));
  }

  if (lotId) tickets = tickets.filter(t => t.lotId === lotId);
  if (spotId) tickets = tickets.filter(t => t.spotId === spotId);

  if (from) {
    const fromDate = new Date(from);
    tickets = tickets.filter(t => new Date(t.entryTime) >= fromDate);
  }

  if (to) {
    const toDate = new Date(to);
    tickets = tickets.filter(t => new Date(t.entryTime) <= toDate);
  }

  res.json(withPaging(tickets, { query: req.query }));
});

// ====== REPORTING ======

// GET /api/reporting/lotsnapshot
server.get('/reporting/lotsnapshot', (req, res) => {
  const db = router.db;
  const lots = db.get('parkingLots').value();
  const spots = db.get('parkingSpots').value();
  const tickets = db.get('tickets').value();
  const payments = db.get('payments').value();

  const todayStr = new Date().toISOString().slice(0, 10); // YYYY-MM-DD

  const snapshot = lots.map(lot => {
    const lotSpots = spots.filter(s => s.lotId === lot.id);
    const lotTickets = tickets.filter(t => t.lotId === lot.id);
    const lotPaymentsToday = payments.filter(p => {
      const d = new Date(p.paidAt).toISOString().slice(0, 10);
      return d === todayStr;
    });

    return {
      lotId: lot.id,
      lotName: lot.name,
      totalSpots: lotSpots.length,
      freeSpots: lotSpots.filter(s => s.status === 0).length,
      occupiedSpots: lotSpots.filter(s => s.status === 1).length,
      unavailableSpots: lotSpots.filter(s => s.status === 2).length,
      openTickets: lotTickets.filter(t => t.status === 0).length,
      revenueToday: lotPaymentsToday.reduce((sum, p) => sum + p.amount, 0)
    };
  });

  res.json({ data: snapshot });
});

// GET /api/reporting/dailyrevenue?from=YYYY-MM-DD&to=YYYY-MM-DD
server.get('/reporting/dailyrevenue', (req, res) => {
  const db = router.db;
  const payments = db.get('payments').value();
  const fromStr = req.query.from;
  const toStr = req.query.to;

  let from = fromStr ? new Date(fromStr) : null;
  let to = toStr ? new Date(toStr) : null;

  const map = new Map();

  payments.forEach(p => {
    const d = new Date(p.paidAt);
    if (from && d < from) return;
    if (to && d > to) return;

    const key = d.toISOString().slice(0, 10); // YYYY-MM-DD
    const prev = map.get(key) || 0;
    map.set(key, prev + p.amount);
  });

  const result = Array.from(map.entries())
    .sort((a, b) => (a[0] < b[0] ? -1 : 1))
    .map(([date, total]) => ({ date, total }));

  res.json({ data: result });
});

// ====== PAYMENTS BY TICKET ======
server.get('/payments/ticket/:ticketId', (req, res) => {
  const db = router.db;
  const list = db.get('payments').filter({ ticketId: req.params.ticketId }).value();
  res.json({ data: list });
});

// ====== (OPTIONAL) FAKE AUTH LOGIN ======
server.post('/auth/login', (req, res) => {
  const db = router.db;
  const { email, password } = req.body || {};
  const user = db.get('users').find({ email, password }).value();
  if (!user) {
    return res.status(401).json({ code: 'invalid_credentials', message: 'Invalid email or password' });
  }
  res.json({
    token: 'fake-jwt-token',
    user: {
      id: user.id,
      email: user.email,
      role: user.role
    }
  });
});

// ---- Finally, use default router for direct REST on collections if needed ----
server.use(router);

const PORT = 3000;
server.listen(PORT, () => {
  console.log(`JSON API running at http://localhost:${PORT}/api/...`);
});
