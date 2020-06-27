//Public Class UrlValidity

//Public Shared Function IsValid(ByVal Url As String) As Boolean
//Dim sStream As Stream
//Dim URLReq As HttpWebRequest
//Dim URLRes As HttpWebResponse

//Try
//     URLReq = WebRequest.Create(Url)
//     URLRes = URLReq.GetResponse()
//     sStream = URLRes.GetResponseStream()
//     Dim reader As String = New StreamReader(sStream).ReadToEnd()
//     Return True
//Catch ex As Exception
//     ‘Url not valid
//     Return False
//End Try
//End Function

//End Class