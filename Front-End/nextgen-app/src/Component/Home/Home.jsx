import React from 'react'
import Navbar from "../pages/HomePage/Navbar/Navbar"
import Hero from '../pages/HomePage/Hero/Hero'
import AboutUs from '../pages/HomePage/AboutUs/AboutUs'
import Footer from '../pages/HomePage/Footer/Footer'
import Hero2 from '../pages/HomePage/Hero2/Hero2'
import ContenteHM from '../pages/HomePage/ContenteHM/ContenteHM'
const Home = () => {
  return (
    <>
      <Navbar />
      <Hero />
      <AboutUs />
      <ContenteHM />
      <Hero2 />
      <Footer/>
    </>
  )
}

export default Home
