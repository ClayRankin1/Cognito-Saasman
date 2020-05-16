import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { License, LicenseType } from '../schema/license';

@Injectable({
    providedIn: 'root',
})
export class LicenseService extends ApiService<License> {
    protected endPoint = 'Licenses';

    getLicenseByType(licenses: License[], licenseType: LicenseType) {
        return licenses.find((l) => l.licenseTypeId === licenseType);
    }
}
