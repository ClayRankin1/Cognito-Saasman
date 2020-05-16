import { Component, enableProdMode, OnInit } from '@angular/core';
import { environment } from '../../../../environments/environment';
import ArrayStore from 'devextreme/data/array_store';
import DataSource from 'devextreme/data/data_source';
import { AdminService } from '../../admin.service';
import { CreateRestAspNetStore } from '../../../shared/data-sources/data-sources';
import { LookupCacheService } from '../../../shared/services/lookup-cache.service';
import { LookUpItem } from '../../../shared/models';
import CustomStore from 'devextreme/data/custom_store';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss'],
})
export class UsersComponent implements OnInit {
    private url = `${environment.activeURL}api/users`;

    dataSource: CustomStore;
    domainsDataSource: DataSource;
    contactsDataSource: DataSource;

    constructor(
        private adminService: AdminService,
        private lookupCacheService: LookupCacheService
    ) {}

    ngOnInit(): void {
        this.adminService.setTitle('Users');
        this.dataSource = CreateRestAspNetStore({
            key: 'id',
            url: this.url,
            entityName: 'User',
        });

        // TODO: FIXME - Introduce Data Services and client side caching...
        this.lookupCacheService.getLookup('Domains').subscribe((domains: LookUpItem[]) => {
            this.domainsDataSource = new DataSource({
                store: new ArrayStore({
                    key: 'id',
                    data: domains,
                }),
            });
            this.domainsDataSource.load();
        });
    }
}
