import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginFormComponent } from './shared/components';
import { AuthGuard } from '../app/auth/auth.guard';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { DisplayDataComponent } from './pages/display-data/display-data.component';
import { ChatComponent } from './chat/chat.component';
import { LinksComponent } from './admin/links/links.component';
import { ProjectusersComponent } from './admin/projectusers/projectusers.component';
import { ContactsadminComponent } from './admin/contacts/contacts.component';
import { NewusersComponent } from './admin/newusers/newusers.component';
import { ItemtypesComponent } from './admin/types/itemtypes/itemtypes.component';
import { DoctypesComponent } from './admin/types/doctypes/doctypes.component';
import { ActtypesComponent } from './admin/types/acttypes/acttypes.component';
import { WebViewerComponent } from './webviewer/webviewer.component';
import { TexteditorComponent } from './texteditor/texteditor.component';

import { PasswordResetFormComponent } from './shared/components/password-reset-form/password-reset-form.component';
import { PasswordResetEmailFormComponent } from './shared/components/password-reset-email-form/password-reset-email-form.component';

const routes: Routes = [
    {
        path: 'display-data',
        component: DisplayDataComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'documentextractor',
        component: WebViewerComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'texteditor',
        component: TexteditorComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'profile',
        component: ProfileComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'home',
        component: HomeComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'chat',
        component: ChatComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'projectusers',
        component: ProjectusersComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'login',
        component: LoginFormComponent,
    },
    {
        path: 'links',
        component: LinksComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'contactsadmin',
        component: ContactsadminComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'newusers',
        component: NewusersComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'itemtypes',
        component: ItemtypesComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'doctypes',
        component: DoctypesComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'acttypes',
        component: ActtypesComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'login-form',
        component: LoginFormComponent,
    },
    {
        path: 'password-confirm',
        component: PasswordResetFormComponent,
    },
    {
        path: 'password-reset',
        component: PasswordResetEmailFormComponent,
    },
    {
        path: 'admin',
        loadChildren: () => import('./admin/admin.module').then((m) => m.AdminModule),
        // todo: FIXME - Add logic to check for Admin rights in the future
        canActivate: [AuthGuard],
    },
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'home',
    },
    { path: '**', redirectTo: 'home' },
];

//enableTracing: true REMOVE for production!

@NgModule({
    imports: [RouterModule.forRoot(routes, { enableTracing: false })],
    exports: [RouterModule],
})
export class AppRoutingModule {}
