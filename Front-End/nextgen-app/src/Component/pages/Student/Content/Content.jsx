import React from 'react'
import profile from "../../../../assets/profile mycourses.svg";
import cardcontent from "../../../../assets/cardcontetnt img.svg";
import couresimg from "../../../../assets/courseimage.svg";
import hour from "../../../../assets/hour.svg";
import video from "../../../../assets/videoicon.svg";
import visit from "../../../../assets/visit.svg";
import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/css';
import 'swiper/css/autoplay';
import { Autoplay } from 'swiper/modules';
import ContenteHM from '../../HomePage/ContenteHM/ContenteHM';

const Content = () => {
    const ContentData = [
        {
          title: 'ما هو "لوريم إيبسوم" ؟',
          description: 'لوريم إيبسوم(Lorem Ipsum) هو ببساطة نص شكلي (بمعنى أن الغاية هي الشكل وليس المحتوى) ويُستخدم',
          img: cardcontent,
        },
        {
          title: 'ما الهدف من استخدامه؟',
          description: 'يُستخدم لوريم إيبسوم لأنه يعطي توزيعاَ طبيعياَ -إلى حد ما- للأحرف',
          img: cardcontent,
        },
        {
          title: 'من أين يأتي؟',
          description: 'لوريم إيبسوم هو نص مأخوذ من نصوص الأدب اللاتيني الكلاسيكي',
          img: cardcontent,
        },
        {
            title: 'من أين يأتي؟',
            description: 'لوريم إيبسوم هو نص مأخوذ من نصوص الأدب اللاتيني الكلاسيكي',
            img: cardcontent,
          },
          {
            title: 'من أين يأتي؟',
            description: 'لوريم إيبسوم هو نص مأخوذ من نصوص الأدب اللاتيني الكلاسيكي',
            img: cardcontent,
          },
          {
            title: 'من أين يأتي؟',
            description: 'لوريم إيبسوم هو نص مأخوذ من نصوص الأدب اللاتيني الكلاسيكي',
            img: cardcontent,
          },
      ];
      const courses = [
        {
          id: 1,
          category: "تنمية الذات وريادة الأعمال",
          title: "Dasar Pemrograman WEB",
          instructor: "By Hedra Naguib",
          description:
            "Materi pembelajarn mengenai pembuatan website tingkat pemula",
          hours: "4.5",
          lectures: 20,
          visits: "1,900",
        },
        {
          id: 2,
          category: "تنمية الذات وريادة الأعمال",
          title: "Dasar Pemrograman WEB",
          instructor: "By Hedra Naguib",
          description:
            "Materi pembelajarn mengenai pembuatan website tingkat pemula",
          hours: "4.5",
          lectures: 20,
          visits: "1,900",
        },
        {
          id: 3,
          category: "تنمية الذات وريادة الأعمال",
          title: "Dasar Pemrograman WEB",
          instructor: "By Hedra Naguib",
          description:
            "Materi pembelajarn mengenai pembuatan website tingkat pemula",
          hours: "4.5",
          lectures: 20,
          visits: "1,900",
        },
        {
          id: 4,
          category: "تنمية الذات وريادة الأعمال",
          title: "Dasar Pemrograman WEB",
          instructor: "By Hedra Naguib",
          description:
            "Materi pembelajarn mengenai pembuatan website tingkat pemula",
          hours: "4.5",
          lectures: 20,
          visits: "1,900",
        },
        {
          id: 5,
          category: "تنمية الذات وريادة الأعمال",
          title: "Dasar Pemrograman WEB",
          instructor: "By Hedra Naguib",
          description:
            "Materi pembelajarn mengenai pembuatan website tingkat pemula",
          hours: "4.5",
          lectures: 20,
          visits: "1,900",
        },
        {
          id: 6,
          category: "تنمية الذات وريادة الأعمال",
          title: "Dasar Pemrograman WEB",
          instructor: "By Hedra Naguib",
          description:
            "Materi pembelajarn mengenai pembuatan website tingkat pemula",
          hours: "4.5",
          lectures: 20,
          visits: "1,900",
        },
        {
          id: 7,
          category: "تنمية الذات وريادة الأعمال",
          title: "Dasar Pemrograman WEB",
          instructor: "By Hedra Naguib",
          description:
            "Materi pembelajarn mengenai pembuatan website tingkat pemula",
          hours: "4.5",
          lectures: 20,
          visits: "1,900",
        },
        {
          id: 8,
          category: "تنمية الذات وريادة الأعمال",
          title: "Dasar Pemrograman WEB",
          instructor: "By Hedra Naguib",
          description:
            "Materi pembelajarn mengenai pembuatan website tingkat pemula",
          hours: "4.5",
          lectures: 20,
          visits: "1,900",
        },
      ];
  return (
    <>
           <div
              className=" d-flex  align-items-center justify-content-end gap-2 p-3"
              style={{ marginTop: "9%", marginRight: "8%" }}
            >
              <p className=" text-white  align-items-center mt-3 fw-bold">
                مرحبا هدرا , نحن سعداء بعودتك
              </p>
              <img src={profile} alt="profile" />
            </div>

            <section className=' bg-white p-3 '>
                <h3 className=' primary-text text-end p-3'>اخر كورسات</h3>

                <Swiper
      modules={[Autoplay]}
      spaceBetween={20}
      slidesPerView={3}
      autoplay={{ delay: 2500 }}
      loop={true}
      breakpoints={{
        0: { slidesPerView: 1 },
        768: { slidesPerView: 2 },
        1024: { slidesPerView: 3 },
      }}
    >
      {ContentData.map((card, index) => (
        <SwiperSlide key={index}>
          <div className="card p-0 border-0" style={{ maxWidth: "450px" }}>
            <div className="row content-card g-0">
              <div className="col-md-8">
                <div className="card-body p-0">
                  <h5 className="primary-text text-end fw-bold me-3 mt-2">{card.title}</h5>
                  <p className="primary-text text-end me-3">{card.description}</p>
                </div>
              </div>
              <div className="col-md-4">
                <img src={card.img} alt="..." style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
              </div>
            </div>
          </div>
        </SwiperSlide>
      ))}
    </Swiper>

    <div className="d-flex gap-5 p-3 justify-content-center " style={{ direction:"rtl"}}>
          <button className="content-btn ">
            
            كورسات تنمية الذات وريادة الأعمال
          </button>
          <button className="content-btn ">كورسات تطوير المهارات المهنية</button>
          <button className="content-btn ">كورسات البرمجة والتكنولوجيا</button>
          <button className="content-btn ">كورسات الهوايات والإبداع</button>
          <button className="content-btn ">كورسات اللغات والتواصل</button>
          <button className="content-btn ">اخري</button>
        </div>
            </section>


            <section>
                <ContenteHM />
            </section>

            <section>
            <h3 className=' text-end text-white p-3 fw-bold'>تم توصية هذه الكورسات لك</h3>
  <div className=" px-4" style={{ marginTop: "4%" }}>
        <div className="row">
          {courses.map((course) => (
            <div key={course.id} className="col-md-3 mb-4">
              <div className="card">
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
                  <p style={{ fontSize: "20px", fontWeight: "bold" }}>
                    {course.title}
                    <span style={{ color: "#2405F2" }}>
                      {" "}
                      {course.instructor}
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
              <button className="addcourse-btn">اضافة الكورس </button>
            </div>
          ))}
        </div>
      </div>
            </section>
    </>
  )
}

export default Content
