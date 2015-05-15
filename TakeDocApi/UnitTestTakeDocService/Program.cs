﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocService.Document.Interface;

namespace UnitTestTakeDocService
{
    public class Program
    {
        public static void Main()
        {
            Utility.Logger.myLogger.Init();
            TakeDocModel.Environnement.Init(System.Configuration.ConfigurationManager.AppSettings);

            Document.DocumentServiceTest documentTest = new Document.DocumentServiceTest();
            Document.MetaDataServiceTest metaDataTest = new Document.MetaDataServiceTest();
            Security.UserRequestAcountTest requestAccountTest = new Security.UserRequestAcountTest();
            Document.TypeDocumentServiceTest typeDocumentTest = new Document.TypeDocumentServiceTest();
            Workflow.WorkflowNoTest workflowNoTest = new Workflow.WorkflowNoTest();
            Workflow.WorkflowAutoTest workflowAutoTest = new Workflow.WorkflowAutoTest();

            /*documentTest.TestOrdered();
            metaDataTest.TestOrdered();
            requestAccountTest.TestOrdered();
            typeDocumentTest.TestOrdered();*/
            workflowNoTest.TestOrdered();
            workflowAutoTest.TestOrdered();

            Console.WriteLine("End");

        }
    }
}
