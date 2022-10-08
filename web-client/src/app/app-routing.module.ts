import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth-guard/auth.guard';
import { AdminGuard } from './core/guards/admin-guard/admin.guard';
import { IsUserGuard } from './core/guards/user-guard/isuser.guard';

const routes: Routes = [
  { path: "", redirectTo: "auth/home", pathMatch: "full" },
  {
    path: 'user',
    loadChildren: () => import('./modules/user/user.module').then(m => m.UserModule),
    canActivate: [AuthGuard, IsUserGuard]
  },
  {
    path: 'admin',
    loadChildren: () => import('./modules/admin-page/admin-page.module').then(m => m.AdminPageModule),
    canActivate: [AuthGuard, AdminGuard]
  },
  {
    path: 'auth',
    loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule)
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
