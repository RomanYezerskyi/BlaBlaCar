// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  serviceApi: '',

  emailConfirmation: 'http://localhost:4200/auth/emailconfirmation',
  resetPasswordUrl: 'http://localhost:4200/auth/resetpassword',

  //api
  baseApiUrl: 'https://localhost:6001/api/',
  baseIdentityServerUrl: 'https://localhost:5001/api/',

  //geoapify
  geoapifyApiUrl: 'https://api.geoapify.com/v1/geocode/',
  geoapifyFirstApiKey: '32849d6b3d3b480c9a60be1ce5891252',
  geoapifySecondApiKey: 'd06cb6573e1e488d92494d5296611f0c',

  //chat hub
  chatHubConnectionUrl: 'https://localhost:6001/chatHub',
  chatHubMethod: 'JoinToChatMessagesNotifications',
  chatHubHandlerMethod: 'BroadcastMessagesFromChats',

  //notifications hub
  notificationsHubConnectionUrl: 'https://localhost:6001/notify',
  notificationsHubMethod: 'JoinToNotificationsHub',
  notificationsHubHandlerMethod: 'BroadcastNotification',
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
