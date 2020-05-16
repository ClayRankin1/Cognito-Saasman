import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DevExpressModule } from 'src/app/shared/devexpress/dev-express.module';
import { TenantService } from 'src/app/data/service/tenant.service';
import { TenantsComponent } from './page/tenants.component';
import { TenantsRoutingModule } from './tenants.routing.module';
import { DomainsGridComponent } from './components/domains-grid.component';

@NgModule({
    imports: [CommonModule, DevExpressModule, TenantsRoutingModule],
    declarations: [TenantsComponent, DomainsGridComponent],
    providers: [TenantService],
})
export class TenantsModule {}
