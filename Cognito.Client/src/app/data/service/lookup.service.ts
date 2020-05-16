import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Lookup, LookupType } from '../schema/lookup';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { CacheService } from 'src/app/core/services/storage/cache.service';
import { StorageKeys } from 'src/app/core/services';
import { environment } from 'src/environments/environment';
import { StringHelper } from 'src/app/shared/helpers/string-helper';

@Injectable({
    providedIn: 'root',
})
export class LookupService extends ApiService<Lookup> {
    protected endPoint = 'Lookups';

    constructor(http: HttpClient, private cacheService: CacheService) {
        super(http);
    }

    getLookup<TLookup extends Lookup>(lookup: LookupType): Observable<TLookup[]> {
        return this.cacheService
            .getCachedResult(
                StorageKeys.LookupsDataCache,
                () => this.getAll(),
                environment.cacheDurationInMinutes
            )
            .pipe(map((lookups) => lookups[StringHelper.camelCase(LookupType[lookup])]));
    }
}
