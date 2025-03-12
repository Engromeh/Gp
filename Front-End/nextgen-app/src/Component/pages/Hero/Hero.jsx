import React from "react";
import { Link } from "react-router-dom";
import hero1 from "../../../assets/hero1.svg";
import hero2 from "../../../assets/hero2.svg";
import hero3 from "../../../assets/hero3.svg";
import hero4 from "../../../assets/hero4.svg";
import hero5 from "../../../assets/hero5.svg";
const Hero = () => {
  return (
    <div className=" text-white ps-2 pe-2" >
      <div className=" d-flex flex-column flex-md-row align-items-center" style={{marginTop:"7%"}}>
        <div className="container position-relative my-5">
          <div className="row">
            <div className="col-md-6 mx-auto">
              <img src={hero1} alt="Main" className="main-image" />
            </div>
          </div>

          <div className="side-images ">
            <img src={hero2} alt="1" className="side-img" />
            <img src={hero2} alt="2" className="side-img" />
            <img src={hero3} alt="3" className="side-img" />
            <img src={hero2} alt="4" className="side-img" />
            <img src={hero4} alt="5" className="side-img" />
            <img src={hero2} alt="6" className="side-img" />
            <img src={hero5} alt="7" className="side-img" />
            <img src={hero2} alt="8" className="side-img" />
          </div>
        </div>

        <div
          className="w-auto h-auto text-center text-md-end "
          style={{ marginBottom: "5%" }}
        >
          <h1
            className="font-type text-white pe-5"
            style={{ fontSize: "64px", fontWeight: "bold" }}
          >
            NEXTGEN منصة{" "}
          </h1>
          <div className="">
            <p className="dicrapation-hero">
              منصة تعليمية مجانية تقدم مجموعة واسعة من الدورات التدريبية في
              مختلف المجالات، مثل البرمجة، التصميم، اللغات، التسويق، وغيرها.
              يجمع الموقع بين الدورات المجانية المتاحة على الإنترنت من مصادر
              مختلفة، مما يسهل على المستخدمين الوصول إلى المحتوى التعليمي
              المناسب لهم في مكان واحد.
            </p>
          </div>
          <div className="d-flex gap-3 justify-content-end pe-5">
            <Link className="mt-2">كيفية البدء او كيفية الاستخدام؟</Link>
            <button
              style={{
                backgroundColor: "#FCD980",
                color: "#102D4C",
                borderRadius: "10px",
                width: "25%",
                fontWeight: "bold",
              }}
            >
              ابدا التعلم من اليوم
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Hero;
