import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
} from "react-router-dom";

import Home from "../Component/Home/Home";
import Login from "../Component/pages/Login/Login";
import Register from "../Component/pages/Register/Register";
import FAQ from "../Component/pages/Student/FAQ/FAQ";
import Layout from "../Layout/Layout";
import Contactus from "../Component/pages/Student/Contact us/Contactus";
import Mycourses from "../Component/pages/Student/Mycourses/Mycourses.JSX";
import Content from "../Component/pages/Student/Content/Content";
import InsPage from "../Component/pages/Instructor/InsPage";
import InsAddCourse from "../Component/pages/Instructor/InsAddCourse";
import EditCourse from "../Component/pages/Instructor/EditCourse";
import InsContent from "../Component/pages/Instructor/Inscontent";

const Router = createBrowserRouter(
  createRoutesFromElements(
    <>
   <Route path="/" element={<Home />} />
<Route path="/login" element={<Login />} />
<Route path="/register" element={<Register />} />
<Route path="/instructor" element={<InsPage />}>
  <Route path="" element={<InsContent/>} />
  <Route path="addCourse" element={<InsAddCourse />} />
  <Route path="editCourse" element={<EditCourse />} />
</Route>
<Route path="/layout" element ={<Layout />}>
<Route path="/layout/Faq" element={<FAQ />}/>
<Route path="/layout/Contactus" element={<Contactus />}/>
<Route path="/layout/mycourses" element={<Mycourses />}/>
<Route path="/layout/content" element={<Content/>}/>



</Route>   
 </>
  )
);

export default Router;
