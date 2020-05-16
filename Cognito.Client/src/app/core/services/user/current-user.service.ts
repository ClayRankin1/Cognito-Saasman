import { Injectable } from '@angular/core';
import { StorageService } from '../storage/storage.service';
import { StorageKeys } from '../storage/storage-keys';

@Injectable({
    providedIn: 'root',
})
export class CurrentUserService {
    constructor(private storageService: StorageService) {}

    saveRefreshToken(refreshToken: string): void {
        this.storageService.set(StorageKeys.RefreshToken, refreshToken);
    }
}
