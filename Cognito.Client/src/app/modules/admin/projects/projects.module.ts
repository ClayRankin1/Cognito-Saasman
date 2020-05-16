import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DevExpressModule } from 'src/app/shared/devexpress/dev-express.module';
import { TenantService } from 'src/app/data/service/tenant.service';
import { ProjectsRoutingModule } from './projects.routing.module';
import { ProjectsComponent } from './page/projects.component';
import { ProjectsGridComponent } from './components/projects-grid.component';

@NgModule({
    imports: [CommonModule, DevExpressModule, ProjectsRoutingModule],
    declarations: [ProjectsComponent, ProjectsGridComponent],
    providers: [TenantService],
})
export class ProjectsModule {}
