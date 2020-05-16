import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { RoleGuard } from 'src/app/auth/role.guard';
import { UserRoles } from 'src/app/auth/user.roles.enum';
import { ProjectsComponent } from './page/projects.component';

const routes: Routes = [
    {
        path: '',
        component: ProjectsComponent,
        canActivate: [RoleGuard],
        data: {
            allowedDomainRoles: [UserRoles.DomainAdmin]
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProjectsRoutingModule {}
