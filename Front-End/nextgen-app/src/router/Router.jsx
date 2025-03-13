import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
} from "react-router-dom";

import Home from "../Component/Home/Home";
import Login from "../Component/pages/Login/Login";
import Register from "../Component/pages/Register/Register";

const Router = createBrowserRouter(
  createRoutesFromElements(
    <>
   <Route path="/" element={<Home />} />
<Route path="/login" element={<Login />} />
<Route path="/register" element={<Register />} />

      
    </>
  )
);

export default Router;
