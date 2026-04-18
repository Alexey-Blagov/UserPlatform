import Keycloak from "keycloak-js";

const keycloak = new Keycloak({
  url: "http://localhost:8081",
  realm: "user-platform",
  clientId: "frontend-web"
});

export default keycloak;
