
import { NavLink as Link } from 'react-router-dom';
import styled from 'styled-components';
 
export const Nav = styled.nav`
    background: #797979;
    position: fixed;
    top: 0px;
    right: 1px;
    width: 65%;
    height: 45px;
    display: flex;
    justify-content: space-between;
    padding: 0.2rem calc((100vw - 1000px) / 2);
    z-index: 12;
`;
 
export const NavLink = styled(Link)`
    color: #808080;
    display: flex;
    align-items: center;
    text-decoration: none;
    padding: 0 1rem;
    height: 100%;
    cursor: pointer;
    &.active {
        color: #4d4dff;
    }
`;
 
export const NavMenu = styled.div`
    display: flex;
    align-items: center;
   
    @media screen and (max-width: 768px) {
        display: none;
    }
`;