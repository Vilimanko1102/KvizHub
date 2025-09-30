// src/components/AppNavbar.tsx
import React from "react";
import { Navbar, Nav, Container, Button } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const AppNavbar: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    navigate("/landing");
    logout();
  };

  return (
    <Navbar bg="dark" variant="dark" expand="lg" sticky="top">
      <Container>
        {/* brand može direktno da koristi Link */}
        <Navbar.Brand as={Link} to="/">
          KvizHub
        </Navbar.Brand>

        <Navbar.Toggle aria-controls="main-navbar" />
        <Navbar.Collapse id="main-navbar">
          <Nav className="me-auto">
            <Nav.Link as={Link} to="/quizzes">Lista kvizova</Nav.Link>
            <Nav.Link as={Link} to="/profile">Profil</Nav.Link>
            <Nav.Link as={Link} to="/my-attempts">Moji rezultati</Nav.Link> {/* NOVO */}
            {user?.role === "Admin" && (
                <Nav.Link as={Link} to="/admin">Admin</Nav.Link>
            )}
          </Nav>

          <Nav className="ms-auto d-flex align-items-center gap-2">
            {user ? (
              <>
                <span className="text-light me-2">Zdravo, {user.username}</span>
                <Button variant="outline-danger" onClick={handleLogout}>
                  Izloguj se
                </Button>
              </>
            ) : (
              // koristimo običan Link stilizovan kao dugme — izbegavamo `Button as={Link}` TS problem
              <Link to="/landing" className="btn btn-outline-success">
                Prijavi se
              </Link>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default AppNavbar;
