import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { SideNavOuterToolbarComponent, SingleCardComponent } from './layouts';
import {
    FooterComponent,
    LoginFormComponent,
    HeaderComponent,
    UserPanelComponent,
    SideNavigationMenuComponent,
} from './shared/components';
import { AppRoutingModule } from './app-routing.module';
import { environment } from '../environments/environment';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { FileuploaderComponent } from './fileuploader/fileuploader.component';
import { ChatComponent } from './chat/chat.component';
import { HomeComponent } from './pages/home/home.component';
import { DocumentsComponent } from './documents/documents.component';
import { WebViewerComponent } from './webviewer/webviewer.component';
import { PointsComponent } from './points/points.component';
import { ContactsComponent } from './contacts/contacts.component';
import { DetailsComponent } from './details/details.component';
import { TasksComponent } from './tasks/tasks.component';
import { DisplayDataComponent } from './pages/display-data/display-data.component';
import { ProfileComponent } from './pages/profile/profile.component';

import { ProjectusersComponent } from './admin/projectusers/projectusers.component';
import { ItemtypesComponent } from './admin/types/itemtypes/itemtypes.component';
import { DoctypesComponent } from './admin/types/doctypes/doctypes.component';
import { ActtypesComponent } from './admin/types/acttypes/acttypes.component';
import { LinksComponent } from './admin/links/links.component';
import { NewusersComponent } from './admin/newusers/newusers.component';
import { ContactsadminComponent } from './admin/contacts/contacts.component';
import { UploaderCommService } from './shared/services/uploader-comm.service';
import { PasswordResetFormComponent } from './shared/components/password-reset-form/password-reset-form.component';
import { PasswordResetEmailFormComponent } from './shared/components/password-reset-email-form/password-reset-email-form.component';
import { SideNavigationMenuChatComponent } from './shared/components/side-navigation-menu-chat/side-navigation-menu-chat.component';
import { TexteditorComponent } from './texteditor/texteditor.component';
import { ReportViewerComponent } from './report-viewer/report-viewer.component';
import { WebsiteComponent } from './website/website.component';
import { RouterModule } from '@angular/router';
import { DevExpressModule } from './shared/devexpress/dev-express.module';
import { CACHED_LOOKUP_MAPPINGS } from './shared/services/lookup-cache.service';
import { LookUpItem } from './shared/models';
import { AdminModule } from './admin/admin.module';
import { CoreModule } from './core/core.module';
import { AuthInterceptor, RefreshTokenInterceptor, ErrorInterceptor } from './shared/interceptors';
import { LookupService } from './data/service/lookup.service';
import { AuthState } from './shared/state/auth/auth.state';
import { DomainState } from './shared/state/domain/domain.state';
import { ProjectState } from './shared/state/project/project.state';
import { NgxsModule } from '@ngxs/store';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';

// TODO: Jiri's code
export function domainCallback(item: any): LookUpItem {
    return { id: item.id, label: item.name };
}

@NgModule({
    declarations: [
        AppComponent,
        FileuploaderComponent,
        ChatComponent,
        HomeComponent,
        HeaderComponent,
        ProfileComponent,
        ProjectusersComponent,
        DisplayDataComponent,
        TasksComponent,
        DetailsComponent,
        PointsComponent,
        ContactsComponent,
        WebViewerComponent,
        DocumentsComponent,
        ProjectusersComponent,
        ItemtypesComponent,
        DoctypesComponent,
        ActtypesComponent,
        LinksComponent,
        NewusersComponent,
        ContactsadminComponent,
        SingleCardComponent,
        FooterComponent,
        PasswordResetFormComponent,
        PasswordResetEmailFormComponent,
        LoginFormComponent,
        UserPanelComponent,
        SideNavOuterToolbarComponent,
        SideNavigationMenuComponent,
        SideNavigationMenuChatComponent,
        TexteditorComponent,
        ReportViewerComponent,
        WebsiteComponent,
    ],
    imports: [
        NgxsModule.forRoot([AuthState, DomainState, ProjectState], {
            developmentMode: !environment.production,
        }),
        NgxsStoragePluginModule.forRoot(),
        NgxsReduxDevtoolsPluginModule.forRoot(),
        CoreModule,
        CommonModule,
        BrowserModule,
        FormsModule,
        HttpClientModule,
        AppRoutingModule,
        RouterModule,
        DevExpressModule,
        // todo: FIXME - this should be removed,
        // not sure why module is not lazy loaded when deployed onto AWS
        AdminModule,
    ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: RefreshTokenInterceptor,
            multi: true,
        },
        {
            provide: CACHED_LOOKUP_MAPPINGS,
            useValue: [
                {
                    lookupName: 'Domains',
                    url: 'api/domains',
                    mapCallback: domainCallback,
                },
            ],
        },
        UploaderCommService,
    ],
    bootstrap: [AppComponent],
})
export class AppModule {
    constructor() {
        if (environment.apiDev) {
            environment.activeURL = environment.apiDevURL;
        } else {
            environment.activeURL = environment.clientDevURL;
        }
    }
}
