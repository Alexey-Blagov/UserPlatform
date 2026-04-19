import { render, screen } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { describe, expect, it, vi } from "vitest";
import App from "./App";

vi.mock("./api", () => ({
  api: {
    get: vi.fn().mockRejectedValue(new Error("not mocked")),
    put: vi.fn()
  }
}));

describe("App", () => {
  it("renders the profile route with user data and navigation", async () => {
    const keycloak = {
      logout: vi.fn(),
      tokenParsed: {
        preferred_username: "alexey",
        email: "alexey@example.com",
        realm_access: {
          roles: ["user"]
        }
      }
    } as any;

    render(
      <MemoryRouter initialEntries={["/"]}>
        <App keycloak={keycloak} />
      </MemoryRouter>
    );

    expect(screen.getByRole("heading", { name: "Профиль пользователя" })).toBeInTheDocument();
    expect(screen.getByText("alexey")).toBeInTheDocument();
    expect(screen.getByText("alexey@example.com")).toBeInTheDocument();
    expect(screen.getByRole("link", { name: "Админ" })).toBeInTheDocument();
    expect(screen.getByRole("button", { name: "Выйти" })).toBeInTheDocument();
  });
});
