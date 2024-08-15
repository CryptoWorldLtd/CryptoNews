import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Filter from '../Components/Filter';
import '../Pages/News.css';
import axiosInstance from '../utils/axiosInstance';
import { NavLink as Link } from 'react-router-dom';

const News = () => {
    const [news, setNews] = useState([]);
    const [filters, setFilters] = useState({ Category: '', Region: '', StartDate: '', EndDate: '', SearchTerm: '', Sorting: '' });
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);
    const [initialized, setInitialized] = useState(false);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            setIsAuthenticated(true);
        }

        const params = new URLSearchParams(window.location.search);
        const queryPage = parseInt(params.get('currentPage')) || 1;
        const queryFilters = {
            Category: params.get('category') || '',
            Region: params.get('region') || '',
            StartDate: params.get('startDate') || '',
            EndDate: params.get('endDate') || '',
            SearchTerm: decodeURIComponent(params.get('searchTerm') || ''),
            Sorting: params.get('sorting') || ''
        };

        setPage(queryPage);
        setFilters(queryFilters);

        setInitialized(true);
    }, []);

    useEffect(() => {
        if (initialized) {
            fetchNews(page, filters);
        }
    }, [filters, page]);

    const validFilters = Object.fromEntries(
        Object.entries(filters).filter(([key, value]) => value !== null && value !== '')
    );

    const fetchNews = (page, filters) => {
        const query = new URLSearchParams({
            currentPage: page,
            ...validFilters
        }).toString();
        window.history.pushState(null, '', `?${query}`);

        axiosInstance.get(`https://localhost:7249/News/filter?${query}`)
            .then(response => {
                const fetchedNews = response.data;
                setIsAuthenticated(true);
                if (fetchedNews.length < 1) {
                    setHasMore(false);
                }
                else {
                    setHasMore(true);
                }
                setNews(fetchedNews);
            })
            .catch(error =>
                console.error('Error fetching news', error),
                setIsAuthenticated(false));
    };

    const handleFilterChange = (newFilters) => {
        setFilters(newFilters);
        setPage(1);
        setHasMore(true);
    };

    const handlePageChange = (page) => {
        setPage(page);
    };

    return (
        <div>
            {isAuthenticated ? (
                <div className="restricted-section">
                    <div className="news-page">
                        <Filter onFilterChange={handleFilterChange} />
                        <div className="pagination">
                            <button onClick={() => handlePageChange(page - 1)} disabled={page === 1}>Previous</button>
                            <span>{hasMore ? page : 'No More Pages'}</span>
                            <button onClick={() => handlePageChange(page + 1)} disabled={!hasMore}>Next</button>
                        </div>
                        <div className="news-list">
                            {news.map((article) => (
                                <div key={article.id} className="news-item${id}">
                                    <img src={article.imageUrl} height="500"></img>
                                    <h3>{article.title}</h3>
                                    <p>{article.content}</p>
                                    <p>Date of publication: {article.datePublished}</p>
                                    <p>Rating popularity: {article.rating}</p>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            )
                : (
                    <div className="login-prompt">
                        <h2>Login Required</h2>
                        <p>Please login to view exclusive content.</p>
                        <div className="login">Click <Link to={'/Login'}>here</Link> to go to Login page</div>
                    </div>
                )}
        </div>
    );
};

export default News;