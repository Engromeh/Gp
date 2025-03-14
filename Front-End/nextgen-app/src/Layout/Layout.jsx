import React from 'react'
import NavbarStudent from '../Component/pages/Student/NavbarStudent/NavbarStudent'
import { Outlet } from 'react-router-dom'
import Footer from '../Component/pages/HomePage/Footer/Footer'

const Layout = () => {
  return (
    <div>

    <NavbarStudent />
<Outlet />
<Footer />
    </div>
  )
}

export default Layout
