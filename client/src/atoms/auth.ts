import { useNavigate } from "react-router-dom";
import { atom, useAtom } from "jotai";
import { Api, AuthUserInfo } from "../api";
import { http, TOKEN_KEY } from "../http";
import { atomWithStorage } from "jotai/utils";

export type Credentials = { email: string; password: string };

type AuthHook = {
  user: AuthUserInfo | null;
  login: (credentials: Credentials) => Promise<void>;
  logout: () => void;
};

const jwtAtom = atomWithStorage<string | null>(TOKEN_KEY, null);

const userInfoAtom = atom(async (get) => {
  // Create a dependency on 'token' atom
  const token = get(jwtAtom);
  if (!token) return null;
  // Fetch user-info
  const response = await http.authUserinfoList();
  return response.data;
});

export const useAuth = () => {
  const [_, setToken] = useAtom(jwtAtom);
  const [user] = useAtom(userInfoAtom);
  const navigate = useNavigate();

  const login = async (credentials: Credentials) => {
    const response = await new Api().api.authLoginCreate(credentials);
    const data = response.data;
    setToken(data.jwt!);
    navigate("/");
  };

  const logout = async () => {
    setToken(null);
    navigate("/login");
  };
  return {
    user,
    login,
    logout,
  } as AuthHook;
};
