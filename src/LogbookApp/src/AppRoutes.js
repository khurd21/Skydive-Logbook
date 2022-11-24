import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import Home from "./components/Home";
import { Logbook } from "./components/Logbook";
import PersonalInfo from "./components/PersonalInfo";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/logbook',
    requireAuth: true,
    element: <Logbook />
  },
  {
    path: '/personal-info',
    requireAuth: true,
    element: <PersonalInfo />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
