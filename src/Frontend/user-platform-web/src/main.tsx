import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import keycloak from "./keycloak";
import "./styles.css";

keycloak.init({
  onLoad: "login-required",
  checkLoginIframe: false
}).then(authenticated => {
  if (!authenticated) {
    keycloak.login();
    return;
  }

  ReactDOM.createRoot(document.getElementById("root")!).render(
    <React.StrictMode>
      <BrowserRouter>
        <App keycloak={keycloak} />
      </BrowserRouter>
    </React.StrictMode>
  );
});
