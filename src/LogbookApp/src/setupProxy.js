const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:50980';

const context =  [
  "/api/v1/weatherforecast",
  "/api/v1/logbook",
  "/signin-google",
  "/_configuration",
  "/.well-known",
  "/Identity",
  "/connect",
  "/ApplyDatabaseMigrations",
  "/_framework"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};
