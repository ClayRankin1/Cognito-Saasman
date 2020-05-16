import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { TenantsComponent } from './page/tenants.component';
import { RoleGuard } from 'src/app/auth/role.guard';
import { UserRoles } from 'src/app/auth/user.roles.enum';

const routes: Routes = [
    {
        path: '',
        component: TenantsComponent,
        canActivate: [RoleGuard],
        data: {
            allowedRoles: [UserRoles.SysAdmin],
            allowedDomainRoles: [UserRoles.TenantAdmin]
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TenantsRoutingModule {}
