import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Dashboard } from './pages/dashboard/dashboard';
import { TicketsList } from './pages/tickets/tickets-list/tickets-list';
import { TariffView } from './pages/tariff/tariff-view/tariff-view';
import { FinancialReports } from './pages/financial-reports/financial-reports';
import { Spots } from './pages/spots/spots';
import { LotsList } from './pages/lots/lots-list/lots-list';
import { UsersList } from './pages/users/users-list/users-list';

export const routes: Routes = [
    {path: '', redirectTo: '/login', pathMatch: 'full'},
    {path: 'login', component: Login, title: 'Log In'},
    {path: 'dashboard', component: Dashboard, title: 'Dashboard'},
    {path: 'dashboard/ticket', component: TicketsList, title: 'Tickets'},
    {path: 'dashboard/tariff', component: TariffView, title: 'Tariffs'},
    {path: 'dashboard/financial-report', component: FinancialReports, title: 'Financial Report'},
    {path: 'dashboard/spots', component: Spots, title: 'Parking Spots'},
    {path: 'dashboard/lots', component: LotsList, title: 'Parking Lots'},
    {path: 'dashboard/user', component: UsersList, title: 'Profile'}
];
