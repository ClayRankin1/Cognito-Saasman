import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UsersComponent } from './pages/users/users.component';
import { AdminComponent } from './admin.component';

const routes: Routes = [
    {
        path: '',
        component: AdminComponent,
        children: [
            {
                path: 'tenants',
                loadChildren: (): any => import('../modules/admin/tenants/tenants.module').then((m) => m.TenantsModule),
            },
            {
                path: 'users',
                component: UsersComponent,
            },
            {
                path: 'projects',
                loadChildren: (): any => import('../modules/admin/projects/projects.module').then((m) => m.ProjectsModule),
            },
            { path: '**', redirectTo: 'domains' },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AdminRoutingModule {}
