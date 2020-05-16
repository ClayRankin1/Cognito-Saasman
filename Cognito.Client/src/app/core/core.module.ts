import { NgModule } from '@angular/core';
import { StorageService, CurrentUserService } from './services';

@NgModule({
    providers: [StorageService, CurrentUserService],
})
export class CoreModule {}
