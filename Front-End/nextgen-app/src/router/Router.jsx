import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
} from "react-router-dom";

import Home from "../Component/Home/Home";

const Router = createBrowserRouter(
  createRoutesFromElements(
    <>
   <Route path="/" element={<Home />} />


      
    </>
  )
);

export default Router;
