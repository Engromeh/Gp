import React, { useState } from "react";
import { Swiper, SwiperSlide } from "swiper/react";
import "swiper/css";
import "swiper/css/navigation";
import { Navigation } from "swiper/modules";
import couresimg from "../../../../assets/courseimage.svg";
import hour from "../../../../assets/hour.svg";
import video from "../../../../assets/videoicon.svg";
import visit from "../../../../assets/visit.svg";

const courses = [
  {
    id: 1,
    category: "تنمية الذات وريادة الأعمال",
    title: "Dasar Pemrograman WEB",
    instructor: "By Hedra Naguib",
    description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
    hours: "4.5",
    lectures: 20,
    visits: "1,900",
  },
  {
    id: 2,
    category: "تنمية الذات وريادة الأعمال",
    title: "Dasar Pemrograman WEB",
    instructor: "By Hedra Naguib",
    description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
    hours: "4.5",
    lectures: 20,
    visits: "1,900",
  },
  {
    id: 3,
    category: "تنمية الذات وريادة الأعمال",
    title: "Dasar Pemrograman WEB",
    instructor: "By Hedra Naguib",
    description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
    hours: "4.5",
    lectures: 20,
    visits: "1,900",
  },
  {
    id: 4,
    category: "تنمية الذات وريادة الأعمال",
    title: "Dasar Pemrograman WEB",
    instructor: "By Hedra Naguib",
    description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
    hours: "4.5",
    lectures: 20,
    visits: "1,900",
  },
  {
    id: 5,
    category: "تنمية الذات وريادة الأعمال",
    title: "Dasar Pemrograman WEB",
    instructor: "By Hedra Naguib",
    description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
    hours: "4.5",
    lectures: 20,
    visits: "1,900",
  },
  {
    id: 6,
    category: "تنمية الذات وريادة الأعمال",
    title: "Dasar Pemrograman WEB",
    instructor: "By Hedra Naguib",
    description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
    hours: "4.5",
    lectures: 20,
    visits: "1,900",
  },
  {
    id: 7,
    category: "تنمية الذات وريادة الأعمال",
    title: "Dasar Pemrograman WEB",
    instructor: "By Hedra Naguib",
    description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
    hours: "4.5",
    lectures: 20,
    visits: "1,900",
  },
];

const ContenteHM = () => {
  return (
    <>

 <div className="d-flex gap-5 p-3 justify-content-center " style={{ direction:"rtl"}}>  
 <Swiper
    spaceBetween={10}
    slidesPerView={4}
    breakpoints={{
      768: { slidesPerView: 2 },
      1024: { slidesPerView: 3 },
      1440: { slidesPerView: 5 },
    }}
    className=" d-flex justify-content-center px-4"
  >
    <SwiperSlide>
      <button className="content-btn">
        كورسات تنمية الذات وريادة الأعمال
      </button>
    </SwiperSlide>
    <SwiperSlide>
      <button className="content-btn">كورسات تطوير المهارات المهنية</button>
    </SwiperSlide>
    <SwiperSlide>
      <button className="content-btn">كورسات البرمجة والتكنولوجيا</button>
    </SwiperSlide>
    <SwiperSlide>
      <button className="content-btn">كورسات الهوايات والإبداع</button>
    </SwiperSlide>
    <SwiperSlide>
      <button className="content-btn">كورسات اللغات والتواصل</button>
    </SwiperSlide>
    <SwiperSlide>
      <button className="content-btn">أخرى</button>
    </SwiperSlide>
  </Swiper>
</div>


      <div className="p-3">
      <Swiper
  spaceBetween={10}
  slidesPerView={3}
  breakpoints={{
    768: { slidesPerView: 2 },
    1024: { slidesPerView: 3 },
    1440: { slidesPerView: 4 },
  }}
>

          {courses.map((course) => (
            <SwiperSlide key={course.id}>
              <div
                className="card"
                style={{ width: "100%", marginTop: "20px" }}
              >
                <div
                  className="position-relative"
                  style={{ borderRadius: "12px", overflow: "hidden" }}
                >
                  <img
                    src={couresimg}
                    alt="course"
                    className="w-100"
                    style={{ borderRadius: "12px" }}
                  />

                  <div
                    className="position-absolute d-flex align-items-center px-3 py-1"
                    style={{
                      bottom: "2px",
                      right: "0px",
                      backgroundColor: "#252641",
                      color: "#fff",
                      borderRadius: "20px",
                      fontWeight: "bold",
                    }}
                  >
                    ⭐ 4.9
                  </div>
                </div>

                <div className="card-body">
                  <p style={{ fontSize: "24px" }}>
                    {course.title}
                    <span style={{ color: "#2405F2" }}>
                      {course.instructor}{" "}
                    </span>
                  </p>
                  <p className="card-text">{course.description}</p>
                  <div className="d-flex justify-content-between">
                    <p
                      className="d-flex align-items-center gap-1"
                      style={{ fontWeight: "bold", color: "#282938" }}
                    >
                      <img src={hour} alt="ساعات" style={{ height: "20px" }} />{" "}
                      {course.hours} ساعات
                    </p>
                    <p
                      className="d-flex align-items-center gap-1"
                      style={{ fontWeight: "bold", color: "#282938" }}
                    >
                      <img
                        src={video}
                        alt="محاضرات"
                        style={{ height: "20px" }}
                      />{" "}
                      {course.lectures} محاضرة
                    </p>
                    <p
                      className="d-flex align-items-center gap-1"
                      style={{ fontWeight: "bold", color: "#282938" }}
                    >
                      <img src={visit} alt="زيارات" /> {course.visits} زيارة
                    </p>
                  </div>
                </div>
              </div>
              <button className="addcourse-btn">add course </button>
            </SwiperSlide>
          ))}
        </Swiper>
      </div>
    </>
  );
};

export default ContenteHM;
