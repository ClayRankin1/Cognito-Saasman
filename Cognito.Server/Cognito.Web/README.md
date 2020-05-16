
#How I got local npm start to run https - and use remote API
#via Azure add to CORS: https://localhost:4200
https://medium.com/@rubenvermeulen/running-angular-cli-over-https-with-a-trusted-certificate-4a0d5f92747a
--MUST CHANGE PACKAGE.JSON TO : "start": "ng serve --ssl true",



#Legal document signing
https://go.docusign.com/sandbox/productshot/?elqCampaignId=11223&utm_source=google&utm_medium=cpc&utm_campaign=developer_api_primary&utm_term=docusign%20api&utm_content=domestic_US&gclid=CjwKCAjw5fzrBRASEiwAD2OSVz3oL1JlcaJ3KXqSHX_lxah-i-544FwGe7zfR_WlLKHRO642fw-4pxoC3m8QAvD_BwE



https://colors.muz.li/palette/0e5485/b7b0a6/27211b/80b8dd/2c9bd2


# MAJOR PROJECT MILESTONES

COPY DB TO TEST/DEV VERSION!!

GET DOCUVIEWWARE TO WORK

GET REPORTS TO WORK - (run mvc page from angular link? > via API > View?)

GET UPLOADER TO WORK - (run mvc page from angular link? > via API > View?)

GET LOGIN TO WORK WITH IDENTITY:
will be forced to work-around since you cannot bring passwords over to .net core
need to attempt to add asp.net identity - but pretty sure it wont work
??? If you migrate the auth data as-is - keeping the same userID's - will that not work?
It would just require a password reset (and they could use the same passords if they wanted to)

new password field/hash:
https://www.npmjs.com/package/password-hash

migrating:
https://docs.microsoft.com/en-us/aspnet/core/migration/proper-to-2x/membership-to-core-identity?view=aspnetcore-2.2#next-steps

??JUST .net core user auth?!
https://github.com/cornflourblue/aspnet-core-registration-login-api
https://jasonwatmore.com/post/2018/06/26/aspnet-core-21-simple-api-for-authentication-registration-and-user-management

CHAT BASICS
socket.io is a library that can switch what is used behind the scenes - e.g. between websockets and long polling. etc.
https://www.youtube.com/watch?v=vpQDkEgO-kA -- includes ref to git code: https://github.com/AzharHusain/realtimechatapp


 SQL to Firebase?

 Invoke a report from an API endpoint?
	Add a "page" to .net core
	invoke the page from an end-point...??


#automatic angular documentation(but issues with ng 8):
https://github.com/compodoc/compodoc/issues

# Bugs
FIXED: refresh page logs out user - used local storage...

TMP fix: Login > page refresh causes login without logging in(!)

chat message text box leaves carriage return when enter key pressed to send chat:
try this:
$("textarea").keydown(function(e){
// Enter was pressed without shift key
if (e.keyCode == 13 && !e.shiftKey)
{
    // prevent default behavior
    e.preventDefault();
}
});

from https://stackoverflow.com/questions/18779322/disable-new-line-in-textarea-when-pressed-enter



# HOW-TO AND FIXES
#================
Icons: https://js.devexpress.com/Documentation/Guide/Themes_and_Styles/Icons/
e.g. icon="save"

#================
Get previous version of repo!
Go to repos
Go to commits
in far right column see "<>" then click
from here you can clone or download the repo at this point in history!
#login messed up styling 
showing app main in back - I had removed condtional ngIf - which keeps the main app screen shelll from showing!
#===================

1. Issue: Server hot updates not working
https://github.com/aspnet/JavaScriptServices/issues/1654
https://stackoverflow.com/questions/50867385/development-server-hot-updates-not-working
THIS WORKED!
delete /dist folder
(didn't do this but - may need to add "debug": "ng serve --watch", to package.json)
#==========

FireStore:
Find config setting for environment > Firebase admin site > app > Settings > bottom: "Firebase SDK snippet" select Config!

