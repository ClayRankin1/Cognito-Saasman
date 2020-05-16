import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Tenant } from '../schema/tenant';
import { Observable } from 'rxjs';
import { Domain } from '../schema/domain';

@Injectable({
    providedIn: 'root',
})
export class TenantService extends ApiService<Tenant> {
    protected endPoint = 'Tenants';

    public getDomainsByTenantId(tenantId: number): Observable<Domain[]> {
        return this.http.get<Domain[]>(this.buildUrl([tenantId, 'domains']));
    }
}
