# JWT configuration

Do not place the signing key in source control. Before running the API, set these environment variables:

```powershell
$env:Jwt__Key = '<a-random-secret-of-at-least-32-characters>'
$env:Jwt__Issuer = 'RegSystemAPI'
$env:Jwt__Audience = 'RegSystemFrontend'
```

The frontend development API URL is configured in `reg-system/src/environments/environment.ts`; production uses same-origin `/api` so its web server must proxy that route to this API.
