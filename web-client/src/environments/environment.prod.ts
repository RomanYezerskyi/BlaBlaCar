export const environment = {
  production: true,
  baseApiUrl: 'https://localhost:6001/api/',
  baseIdentityServerUrl: 'https://localhost:5001/api/',
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
