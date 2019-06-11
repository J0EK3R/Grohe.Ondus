﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Grohe.Ondus.Api.Client
{
  public class ApiResponse<T>
    where T : class
  {
    #region Fields
    private T mappedContent;
    private int statusCode;
    private string content;
    #endregion

    #region ApiResponse(HttpResponse httpResponse)
    public ApiResponse(HttpResponseMessage httpResponse)
    {
      // this.statusCode = httpResponse.getStatusLine().getStatusCode();
      this.statusCode = (int)httpResponse.StatusCode;

      //HttpEntity responseEntity = httpResponse.getEntity();

      try
      {
        if (statusCode != 200)
        {
          mappedContent = null;
        }
        else
        {
          extractContentFromResponse(httpResponse);
          // ObjectMapper mapper = new ObjectMapper();
          // mappedContent = mapper.readValue(content, targetClass);
          mappedContent = JsonConvert.DeserializeObject<T>(content);
        }
      }
      finally
      {
        //EntityUtils.consume(responseEntity);
      }
    }
    #endregion

    #region extractContentFromResponse(HttpResponse httpResponse)
    private void extractContentFromResponse(HttpResponseMessage httpResponse)
    {
      //BufferedInputStream bis = new BufferedInputStream(httpResponse.getEntity().getContent());
      //ByteArrayOutputStream buf = new ByteArrayOutputStream();
      //int result = bis.read();
      //while (result != -1)
      //{
      //  buf.write((byte)result);
      //  result = bis.read();
      //}
      //this.content = buf.toString();

      Task<string> httpRequest = httpResponse.Content.ReadAsStringAsync();
      this.content = httpRequest.Result;
    }
    #endregion

    #region getStatusCode()
    public int getStatusCode()
    {
      return statusCode;
    }
    #endregion

    #region getContent()
    public T getContent()
    {
      return mappedContent;
    }
    #endregion

    #region getContentAs<E>()
    public E getContentAs<E>()
      where E : class 
    {
      E contentForTargetClass = null;
      try
      {
        // contentForTargetClass = new ObjectMapper().readValue(this.content, targetClass);
        contentForTargetClass = JsonConvert.DeserializeObject<E>(this.content);
      }
      catch (Exception ignored)
      {
      }
      return contentForTargetClass;
    }
    #endregion
  }
}
