import React from "react";
import share from "../../../assets/sharenlogin.svg";
import loginbg from "../../../assets/loginbg.svg";

import { Link } from "react-router-dom";
const Login = () => {
  return (
    <div
      className="container-fluid d-flex justify-content-center align-items-center min-vh-100"
      style={{ backgroundColor: "#102D4D" }}
    >
      <Link
        to={"/"}
        className=" d-flex align-items-center gap-1"
        style={{
          fontSize: "20px",
          fontWeight: "bold",
          color: "#D9D9D9",
          position: "absolute",
          top: "9%",
          left: "21%",
        }}
      >
        <img src={share} alt="المحتويات" style={{ height: "10px" }} />
        الصفحة الرئيسية{" "}
      </Link>
      <div
        className="row  shadow-lg rounded-4 overflow-hidden "
        style={{ width: "60%" }}
      >
        <div className="col-md-6 d-flex flex-column justify-content-center align-items-center text-white p-5">
          <img src={loginbg} alt="login-image" style={{width:"100%"}}/>
        </div>

        <div className="col-md-6 p-5  text-white">
          <h4 className="fw-bold text-end">تسجيل الدخول</h4>
          <p className="text-end">
            "أنشئ حسابك الآن وانطلق في رحلة التعلم والتطوير!"
          </p>
          <form className="mt-4">
            <div className="mb-3">
              <input
                type="email"
                className="form-control  bg-transparent text-white border-secondary"
                placeholder="الايميل"
                style={{ direction: "rtl" }}
              />
            </div>
            <div className="mb-3">
              <input
                type="password"
                className="form-control  bg-transparent text-white border-secondary"
                placeholder="كلمة المرور"
                style={{ direction: "rtl" }}
              />
            </div>
            <button
              type="submit"
              className="btn w-100 text-white fw-bold"
              style={{ backgroundColor: "#FCD980" }}
            >
              تسجيل الدخول
            </button>
            <p className="mt-3 text-center text-light">
              لست لديك حساب؟ <a href="#">أنشئ حسابًا</a>
            </p>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Login;
