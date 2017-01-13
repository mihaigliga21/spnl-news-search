import scrapy
import re
import pyodbc
import datetime

class News_Spider(scrapy.Spider):
    name = 'news_crawl'
    #allowed_domains = ['']   

    def start_requests(self):
        yield scrapy.Request('http://www.softpedia.com/', self.parse)   

    def parse(self, response):
         linkList = []
         for linkF in response.xpath('//a/@href').extract():
             if(re.match('https?://(?:www)?(?:[\w-]{2,255}(?:\.\w{2,6}){1,2})(?:/[\w&%?#-]{1,300})?', linkF)):
                 if linkF not in linkList:
                    linkList.append(linkF)
         print(linkList)
         self.insertIntoDB(linkList)         

    def insertIntoDB(self, linkList):

            server = 'DESKTOP-179PUIN\SQLEXPRESS'
            database = 'SearchNewsCrawlerData'
            username = 'sa'
            password = '1SQL2012PA$$0rd@*oo*#'
            driver= '{SQL Server}'
            cnxn = pyodbc.connect('DRIVER='+driver+';PORT=1433;SERVER='+server+';PORT=1443;DATABASE='+database+';UID='+username+';PWD='+ password)

            for item in linkList:    
                cursor = cnxn.cursor()
                currentTime = datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')                
                cursor.execute("insert into LinksTable(LinkAdress, DateAdded) values('" + item + "', '" + currentTime + "') ")
                cnxn.commit()           

            cnxn.close()