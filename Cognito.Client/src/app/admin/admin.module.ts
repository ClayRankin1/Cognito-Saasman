import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersComponent } from './pages/users/users.component';
import { AdminRoutingModule } from './admin-routing.module';
import { DevExpressModule } from '../shared/devexpress/dev-express.module';
import { AdminComponent } from './admin.component';
import { ProjectsComponent } from './pages/projects/projects.component';
import { ProjectsService } from './pages/projects/projects.service';
import { AdminService } from './admin.service';

@NgModule({
    imports: [CommonModule, DevExpressModule, AdminRoutingModule],
    declarations: [AdminComponent, UsersComponent, ProjectsComponent],
    providers: [ProjectsService],
})
export class AdminModule {}
