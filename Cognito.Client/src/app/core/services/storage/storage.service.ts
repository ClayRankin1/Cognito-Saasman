import { Injectable } from '@angular/core';
import { CachedItem } from './cached-item.model';

@Injectable({
    providedIn: 'root',
})
export class StorageService {
    private readonly storage: Storage = window.localStorage;
    private readonly sessionStorage: Storage = window.sessionStorage;

    public get<T>(key: string, maxAge?: number): T {
        return this.getInternal<T>(this.storage, key, maxAge);
    }

    public set(key: string, value: any, lifetime: number = null): void {
        this.setInternal(this.storage, key, value, lifetime);
    }

    public remove(key: string): void {
        this.removeInternal(this.storage, key);
    }

    public clear(): void {
        this.clearInternal(this.storage);
    }

    public getSession<T>(key: string, maxAge?: number): T {
        return this.getInternal<T>(this.sessionStorage, key, maxAge);
    }

    public setSession(key: string, value: any, lifetime: number = null): void {
        this.setInternal(this.sessionStorage, key, value, lifetime);
    }

    public removeSession(key: string): void {
        this.removeInternal(this.sessionStorage, key);
    }

    public clearSession(): void {
        this.clearInternal(this.sessionStorage);
    }

    public clearAll() {
        this.clear();
        this.clearSession();
    }

    private getInternal<T>(storage: Storage, key: string, maxAge?: number): T | null {
        const item: CachedItem = JSON.parse(storage.getItem(key) || '{}');

        if (!item.value) {
            return;
        }

        if (item.expiresOn && item.expiresOn < new Date().getTime()) {
            this.removeInternal(storage, key);
            return;
        }

        if (maxAge) {
            const age = new Date().getTime() - item.createdOn;
            if (age > maxAge) {
                return;
            }
        }

        return item.value as T;
    }

    private setInternal(
        storage: Storage,
        key: string,
        value: any,
        lifetime: number = null /* in ms */
    ): void {
        const timestamp = new Date().getTime();
        const item: CachedItem = {
            value,
            expiresOn: lifetime ? timestamp + lifetime : null,
            createdOn: timestamp,
        };

        storage.setItem(key, JSON.stringify(item));
    }

    private removeInternal(storage: Storage, key: string): void {
        storage.removeItem(key);
    }

    private clearInternal(storage: Storage): void {
        storage.clear();
    }
}
