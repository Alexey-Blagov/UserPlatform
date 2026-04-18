import { Link, Route, Routes } from "react-router-dom";
import type Keycloak from "keycloak-js";
import { useEffect, useState } from "react";
import { api } from "./api";

type Props = {
  keycloak: Keycloak;
};

type Profile = {
  id: string;
  externalIdentityId: string;
  firstName: string;
  lastName: string;
  country: string;
  city: string;
  street: string;
  house: string;
  postalCode: string;
};

function ProfilePage({ keycloak }: Props) {
  const [profile, setProfile] = useState<Profile | null>(null);
  const [form, setForm] = useState({
    firstName: "Alexey",
    lastName: "Blagov",
    country: "Finland",
    city: "Turku",
    street: "Main Street",
    house: "10A",
    postalCode: "20100"
  });

  const loadProfile = async () => {
    try {
      const { data } = await api.get("/api/profile/me");
      setProfile(data);
    } catch {
      setProfile(null);
    }
  };

  useEffect(() => {
    loadProfile();
  }, []);

  const save = async () => {
    const { data } = await api.put("/api/profile/me/address", form);
    setProfile(data);
  };

  return (
    <div className="card">
      <h1>Профиль пользователя</h1>
      <p><b>Пользователь:</b> {keycloak.tokenParsed?.preferred_username as string}</p>
      <p><b>Email:</b> {keycloak.tokenParsed?.email as string}</p>
      <p><b>Роли:</b> {JSON.stringify((keycloak.tokenParsed as any)?.realm_access?.roles ?? [])}</p>

      <div className="grid">
        {Object.entries(form).map(([key, value]) => (
          <label key={key}>
            <span>{key}</span>
            <input
              value={value}
              onChange={e => setForm(x => ({ ...x, [key]: e.target.value }))}
            />
          </label>
        ))}
      </div>

      <div className="actions">
        <button onClick={save}>Сохранить профиль и адрес</button>
        <button onClick={loadProfile}>Обновить</button>
      </div>

      {profile && (
        <pre>{JSON.stringify(profile, null, 2)}</pre>
      )}
    </div>
  );
}

function AdminPage() {
  const [summary, setSummary] = useState<any>(null);

  const load = async () => {
    const { data } = await api.get("/api/admin/summary");
    setSummary(data);
  };

  return (
    <div className="card">
      <h1>Админ-зона</h1>
      <button onClick={load}>Получить admin summary</button>
      {summary && <pre>{JSON.stringify(summary, null, 2)}</pre>}
    </div>
  );
}

export default function App({ keycloak }: Props) {
  return (
    <div className="layout">
      <nav className="nav">
        <Link to="/">Профиль</Link>
        <Link to="/admin">Админ</Link>
        <button onClick={() => keycloak.logout()}>Выйти</button>
      </nav>

      <Routes>
        <Route path="/" element={<ProfilePage keycloak={keycloak} />} />
        <Route path="/admin" element={<AdminPage />} />
      </Routes>
    </div>
  );
}
