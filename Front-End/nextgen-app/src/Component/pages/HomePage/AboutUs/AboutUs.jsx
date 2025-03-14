import React from "react";
import { Link } from "react-router-dom";
import about1 from "../../../../assets/about1.svg";
import about2 from "../../../../assets/about2.svg";
import about3 from "../../../../assets/about3.svg";
import about4 from "../../../../assets/about4.svg";
import about5 from "../../../../assets/about5.svg";

const AboutUs = () => {
  return (
    <>
      <div className="w-auto h-auto p-4" style={{ backgroundColor: "#EEF4FA" , zIndex:"999"}}>
        <div className="d-flex  justify-content-between gap-1">
          <div className="d-flex flex-column">
            <p
              style={{
                color: "#282938",
                fontWeight: "bold",
                fontSize: "32px",
                marginBottom: "5px",
              }}
            >
              21.000+
            </p>
            <p style={{ fontSize: "18px", color: "#555" }}>Siswa terdaftar</p>
          </div>

          <div className="d-flex flex-column">
            <p
              style={{
                color: "#282938",
                fontWeight: "bold",
                fontSize: "32px",
                marginBottom: "5px",
              }}
            >
              100+
            </p>
            <p style={{ fontSize: "18px", color: "#555" }}>Instruktur Ahli</p>
          </div>

          <div className="d-flex flex-column">
            <p
              style={{
                color: "#282938",
                fontWeight: "bold",
                fontSize: "32px",
                marginBottom: "5px",
              }}
            >
              150+
            </p>
            <p style={{ fontSize: "18px", color: "#555" }}>Kursus Gratis</p>
          </div>

          <p
            className=" d-flex align-items-center "
            style={{
              fontSize: "24px",
              fontWeight: "bold",
              color: "#282938",
              gap: "3px",
            }}
          >
            <img src={about1} alt="الرئيسية" style={{ height: "20px" }} />
            LOREM
          </p>

          <p
            className=" d-flex align-items-center gap-1"
            style={{ fontSize: "24px", fontWeight: "bold", color: "#282938" }}
          >
            <img src={about2} alt="المحتويات" style={{ height: "20px" }} />
            DITLANCE
          </p>
          <p
            className=" d-flex align-items-center gap-1"
            style={{ fontSize: "24px", fontWeight: "bold", color: "#282938" }}
          >
            <img src={about3} alt="المحتويات" style={{ height: "20px" }} />
            OWTHEST
          </p>
          <p
            className=" d-flex align-items-center gap-1"
            style={{ fontSize: "24px", fontWeight: "bold", color: "#282938" }}
          >
            <img src={about4} alt="المحتويات" style={{ height: "20px" }} />
            NEOVASI
          </p>

          <p
            className=" d-flex align-items-center gap-1"
            style={{ fontSize: "24px", fontWeight: "bold", color: "#282938" }}
          >
            <img src={about5} alt="معلومات عنا" style={{ height: "20px" }} />
            ONAGO
          </p>
        </div>

        <div className="d-flex justify-content-between ">
          <button className="aboutus-btn">
            
            كورسات تنمية الذات وريادة الأعمال
          </button>
          <button className="aboutus-btn">كورسات تطوير المهارات المهنية</button>
          <button className="aboutus-btn">كورسات البرمجة والتكنولوجيا</button>
          <button className="aboutus-btn">كورسات الهوايات والإبداع</button>
          <button className="aboutus-btn">كورسات اللغات والتواصل</button>
          <button className="aboutus-btn">اخري</button>
        </div>
      </div>
    </>
  );
};

export default AboutUs;
