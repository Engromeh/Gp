import React from 'react'
import Navbar from '../HomePage/Navbar/Navbar';
import Footer from '../HomePage/Footer/Footer';
import ChatNote from './ChatNote';

import { Outlet } from 'react-router-dom';



function InsPage() {
  return (
    <>
        
        <Navbar contents = "/instructor"  />
        <Outlet/>
        <ChatNote/>
        <Footer/>
    </>
  )
}

export default InsPage;