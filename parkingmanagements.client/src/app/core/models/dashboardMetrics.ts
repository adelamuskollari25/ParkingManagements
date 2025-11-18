export interface DashboardMetrics {
  lotId: string;
  lotName: string;
  totalSpots: number;
  occupiedSpots: number;
  freeSpots: number;
  unavailableSpots: number;
  openTickets: number;
  revenueToday: number;
  lastUpdated: Date;
}

export interface ReveneuSummary {
  date: Date;
  totalRevenue: number;
  ticketCount: number;
}
