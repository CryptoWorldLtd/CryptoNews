import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Filter from '../Components/Filter';

const News = () => {
    const [news, setNews] = useState([]);
    //const [filters, setFilters] = useState({ date: '', popularity: '', category: '' });
    const [filters, setFilters] = useState({ Category: '', Region:'', StartDate:'', EndDate:'', SearchTerm: '', Sorting: '' });
    const [page, setPage] = useState(1);
    const [pageSize] = useState(5);
    const [totalItems, setTotalItems] = useState(0);
    const [totalPages, setTotalPages] = useState(10);

    
    useEffect(() => {
        //fetchNews(1);

        const query = new URLSearchParams({
            page,
            ...filters
        }).toString();

        window.history.pushState(null, '', `?${query}`);
    }, [filters, page]);

    
    const fetchNews = (page) => {
		axios.get(`https://localhost:7249/News/filter?CurrentPage=${page}`)
		.then(response =>
			setNews(response.data))
		.catch(error => console.error('Error fetching news', error));
    };
	
    const handleFilterChange = (newFilters) => {
		setFilters(newFilters);        
    };
	
    const handlePageChange = (newPage) => {		
		if(news.length === 0){
            setPage(newPage-1);	
			fetchNews(page-1);		
		}
		fetchNews(newPage);
		setPage(newPage);      
    };
	
	
    return (
        <div className="news-page">
            <Filter onFilterChange={handleFilterChange} />
            <div className="pagination">
                <button onClick={() => handlePageChange(page - 1)} disabled={page === 1}>Previous</button>
                <span>Page {page}</span>
                <button onClick={() => handlePageChange(page + 1)} disabled={page === totalPages}>Next</button>
            </div>
            <div className="news-list">
                {news.map((article) => (
                    <div key={article.id} className="news-item${id}">
                        <img src={article.imageUrl}  height="500"></img>
                        <h3>{article.title}</h3>
                        <p>{article.content}</p>
                        <p>Date of publication: {article.datePublished}</p>
                        <p>Rating popularity: {article.rating}</p>
                     </div>
                ))}
            </div>
        </div>
    );
};

export default News;
