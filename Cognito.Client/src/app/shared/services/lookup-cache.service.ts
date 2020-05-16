import { Injectable, InjectionToken, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { shareReplay, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { LookUpItem } from '../models';

export type LookupType = 'Domains' | 'Levels';

export interface CachedLookupMapping {
    lookupName: LookupType;
    url: string;
    mapCallback: (value: number) => LookUpItem;
}

export const CACHED_LOOKUP_MAPPINGS = new InjectionToken<CachedLookupMapping[]>(
    'CachedLookupMappings'
);

@Injectable({
    providedIn: 'root',
})
export class LookupCacheService {
    cache: {
        [key in LookupType]?: Observable<LookUpItem[]>;
    } = {};

    constructor(
        private http: HttpClient,
        @Inject(CACHED_LOOKUP_MAPPINGS) private mappings: CachedLookupMapping[]
    ) {}

    getLookup(lookup: LookupType) {
        if (this.cache[lookup]) {
            return this.cache[lookup];
        }

        const mapping = this.mappings.find((m) => m.lookupName === lookup);

        this.cache[lookup] = this.http.get<any[]>(`${environment.activeURL}${mapping.url}`).pipe(
            map((items) => items.map((i) => mapping.mapCallback(i))),
            shareReplay(1)
        );

        return this.cache[lookup];
    }

    invalidateCacheItem(lookup: LookupType) {
        delete this.cache[lookup];
    }
}
