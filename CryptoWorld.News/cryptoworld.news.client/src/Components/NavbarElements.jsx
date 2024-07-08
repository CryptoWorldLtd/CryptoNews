import { NavLink as Link } from 'react-router-dom';
import styled from 'styled-components';

export const Nav = styled.nav`
    background: #797979;
    position: fixed;
    top: 0;
    right: 1px;
    width: 100%;
    height: 45px;
    display: flex;
    justify-content: space-between;
    padding: 9px;
    @media screen and (max-width: 768px) {
        width: 100%;
    }
`;

export const NavLink = styled(Link)`
    color: #ffffff;
    display: flex;
    align-items: center;
    text-decoration: none;
    height: 100%;
    cursor: pointer;
    &.active {
        color: #4d4dff;
    }
`;

export const NavMenu = styled.div`
    display: flex;
    align-items: start;
    
    @media screen and (max-width: 768px) {
        display: none;
    }
`;
