import React from "react";
import img1hero2 from "../../../../assets/hero2img1.svg";
import img2hero2 from "../../../../assets/hero2img2.svg";
import img3hero2 from "../../../../assets/hero2img3.svg";
import img4hero2 from "../../../../assets/hero2img4.svg";

const Hero2 = () => {
  return (
    <>
      <div class="" style={{ backgroundColor: "#EEF4FA", marginBottom: "2%" }}>
        <div class="row" style={{ padding: "2%" }}>
          <div class="col-5">
            <img src={img2hero2} alt="hhero2" />
            <p className="titel-hero2">NEXTGEN منصة</p>
            <p className="dicrapation-hero2">
              It is a long established fact that a reader will be distracted by
              the readable content of a page when looking at its layout. The
              content here',
            </p>
          </div>
          <div class="col-7">
            <img src={img1hero2} alt="hhero2" />
            <p className="titel-hero2">NEXTGEN منصة</p>
            <p className="dicrapation-hero2">
              It is a long established fact that a reader will be distracted by
              <br />
              the readable content of a page when looking at its layout. The
              content here',
            </p>
          </div>
        </div>
        <div class="row" style={{ padding: "2%" }}>
          <div class="col-7">
            <img src={img3hero2} alt="hhero2" />
            <p className="titel-hero2">NEXTGEN منصة</p>
            <p className="dicrapation-hero2">
              It is a long established fact that a reader will be distracted by
              <br />
              the readable content of a page when looking at its layout. The
              content here',
            </p>
          </div>
          <div class="col-5">
            <img src={img4hero2} alt="hhero2" />
            <p className="titel-hero2">NEXTGEN منصة</p>
            <p className="dicrapation-hero2">
              It is a long established fact that a reader will be distracted by
              the readable content of a page when looking at its layout. The
              content here',
            </p>
          </div>
        </div>
      </div>
    </>
  );
};

export default Hero2;
