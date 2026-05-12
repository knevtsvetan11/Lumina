import { RouterModule, Routes } from '@angular/router';
import { MovieDetailComponent } from './shared/components/movies/movie-detail/movie-detail.component';
import { HomeComponent } from './shared/components/home/home.component';
import { CinemaListComponent } from './shared/components/cinemas/cinema-list/cinema-list.component';
import { ScreeningComponent } from './shared/components/screenings/screening.component';
import { WatchlistComponent } from './shared/components/watchlist/watchlist.component';
import { TicketComponent } from './shared/components/tickets/ticket-component';
import { AdminGuard } from './core/guards/admin-guard';
import { AdminComponent } from './admin/admin.component';
import { MovieComponent } from './shared/components/movies/movie.component';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { ManageUsersComponent } from './admin/manage-users/manage-users.component';
import { RoleManageComponent } from './admin/role-management-component/role-manage.component';
import { SpecialPermissionsComponent } from './admin/special-permissions-component/special-permissions.component';
import { ChatComponent } from './shared/components/chat-component/chat.component';
import { AdminChatComponent } from './admin/admin-chat-component/admin-chat.component';

export const routes: Routes = [
  {
    path: 'admin',
    canActivate: [AdminGuard],
    loadComponent: () => import('./admin/admin.component').then((m) => m.AdminComponent),
    /// work here make a guard to safe app
  },
    { path: 'adminchats', component: AdminChatComponent },
  { path: 'chats', component: ChatComponent },
  { path: 'permissions', component: SpecialPermissionsComponent },
  { path: 'roles', component: RoleManageComponent },
  { path: 'users', component: ManageUsersComponent },
  { path: 'movies', component: MovieComponent },
  { path: 'tickets', component: TicketComponent },
  { path: 'ticket/:id', component: TicketComponent },
  { path: 'watchlist', component: WatchlistComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'screenings', component: ScreeningComponent },
  { path: 'screenings/:id', component: ScreeningComponent },
  { path: 'login', component: LoginComponent },
  { path: 'cinemas', component: CinemaListComponent },
  { path: 'home', component: HomeComponent },
  { path: 'movies/:id', component: MovieDetailComponent }, 
  { path: '', redirectTo: '/home', pathMatch: 'full' }, 
];
