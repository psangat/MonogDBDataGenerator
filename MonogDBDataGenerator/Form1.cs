using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

///-----------------------------------------------------------------
///   Namespace:      <MonogDBDataGenerator>
///   Class:          <Form1>
///   Description:    <An application to simulate data for project Zarkov>
///   Author:         <Prajwol Sangat>                    Date: <February 8, 2016>
///   Notes:          <It is a simulation application and does not generate the actual instrument data ;)>
///-----------------------------------------------------------------

namespace MonogDBDataGenerator
{
    public partial class Form1 : Form
    {
        String[] instruments = new string[] { "XTN-750", "1AS-1NQ", "ZAR-KOV", "DEAD-POOL", "XTR-RET" };
        String[] sampleTypes = new String[] { "Type1", "Type2", "Type3", "Type4", "Type5", "Type6", "Type7", "Type8", "Type9", "Type10", "Type11", "Type12", "Type13", "Type14", "Type15", "Type16", "Type17", "Type18", "Type19", "Type20" };
        String[] testTypes = new String[] { "Electronic Noise", "Light Throughput", "0 Abs Noise", "Stray Light (Auto)", "Minimum SBW", "Wavelength Drive Check (Short)", "Wavelength Drive Check (Long)", "Stray Light (Manual)" };
        String[] testMethods = new String[] { "Time Scan", "Single Beam WL Scan" };
        public Object Temp { get; set; }

        public Form1()
        {
            InitializeComponent();

        }

        /* private async void findButton_Click(object sender, EventArgs e)
         {
             DateTime startTime = DateTime.Now;
             DateTime endTime;
             console.AppendText("\nConnecting to Mongo Server ...\n");
             var _client = new MongoClient();
             console.AppendText("Mongo Server : Connected\n");
             var _database = _client.GetDatabase("agilent");
             console.AppendText("Connected to Database: agilent\n");
             console.AppendText("Retrieving the collection: zarkov_meta\n");
             var _collection = _database.GetCollection<BsonDocument>("zarkov_meta");
             //var args = new ParallelScanArgs<BsonDocument> { };

             var _filter = new BsonDocument().Add(DBConstants.EngineType, "Type10");
             // var _filter = Builders<BsonDocument>.Filter.Eq(DBConstants.EngineType, "Type10");
             // var _filter = _builder.Eq(DBConstants.EngineType, "Type10") | _builder.Eq(DBConstants.EngineSpearFWVer, "0.8146");
             // var _filter = _builder.Eq(DBConstants.EngineType, "Type10") & _builder.Eq(DBConstants.EngineSpearFWVer, "0.8146");

             // var _builder = Builders<BsonDocument>.Filter;
             // var _filter = _builder.Eq(DBConstants.EngineType, "Type10");// & _builder.Eq(DBConstants.RawData + "." + DBConstants.SampleGain + "." + DBConstants.UV, 15);
             console.AppendText("zarkov ready to use\n");

             var _sort = new BsonDocument().Add(DBConstants.EndDateTime, -1);
             var _project = new BsonDocument().Add(DBConstants.Data, 1);
             var result = await _collection.Find(_filter).Sort(_sort).Project(_project).Limit(5).ToListAsync();

             ///Parallel.ForEach(_collectionMeta.);
             var count = result.Count;
             /*using (var cursor = await _collectionMeta.FindAsync(_filter))
             {
                console.AppendText("\nPlease wait, Looking for documents");
                 while (await cursor.MoveNextAsync())
                 {
                    console.AppendText("..");
                     var batch = cursor.Current;
                     //Parallel.ForEach(batch, (document) => { count++; });
                     foreach (var document in batch)
                     {
                         // process document
                         count++;
                         //console.console.AppendText(document.ToString());
                     }
                 }
             }
             //var count = result.Count;
             endTime = DateTime.Now;
             TimeSpan ts = endTime.Subtract(startTime);

             console.AppendText("\n========================================================== \n");
             console.AppendText("\nSearch Complete!! \n \nDocuments Found:" + count + "\n \n Time elapsed:" + ts.Minutes + ":" + ts.Seconds + ":" + ts.Milliseconds);
             //_collectionMeta.fo


         }*/

        private void generateDataV3_Click(object sender, EventArgs e)
        {
            /* DateTime startTime = DateTime.Now;
            DateTime endTime;

            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken ct = cts.Token;
                ParallelOptions po = new ParallelOptions { CancellationToken = ct, MaxDegreeOfParallelism = System.Environment.ProcessorCount };

                Parallel.Invoke(po,
                        new Action(() => insertData(ct)),
                        new Action(() => insertData(ct)),
                        new Action(() => insertData(ct)),
                        new Action(() => insertData(ct)),
                        new Action(() => insertData(ct)),
                        new Action(() => insertData(ct)),
                        new Action(() => insertData(ct)),
                        new Action(() => insertData(ct))
                    );
            }
            catch (OperationCanceledException ex)
            {
                console.AppendText(ex.Message);
            }
            finally
            {
                endTime = DateTime.Now;
                TimeSpan ts = endTime.Subtract(startTime);

                console.AppendText("\n========================================================== \n");
                console.AppendText("\n  Complete!! \n \n Time elapsed:" + ts.Minutes + ":" + ts.Seconds + ":" + ts.Milliseconds);
            }*/
            var _collection = connectServerAndGetCollection("agilent1", "zarkov_v3");
            createAndInsertData(_collection);


            //Console.ReadKey();

        }

        private void remoteDataGenereate_Click(object sender, EventArgs e)
        {
             var _collection = connectServerAndGetCollection("agilent", "zarkov_" + DateTime.Now.Month + "_" + DateTime.Now.Year);
             createAndInsertData(_collection);
        }

        private IMongoCollection<BsonDocument> connectServerAndGetCollection(string dbName, string collection)
        {
            try
            {
                return new MongoClient(ConfigurationSettings.AppSettings["MongoDBConectionString"]).GetDatabase(dbName).GetCollection<BsonDocument>(collection);

            }
            catch (Exception ex)
            {
                console.AppendText(String.Format("[Error]: {0}", ex.Message));
                return null;
            }
        }

        private void swapTest() {
            var lis = new List<BsonDocument>();
            var bsdoc = new BsonDocument();
            for (int i = 1; i < 10; i++) {
                bsdoc.Add(DBConstants.Id, Guid.NewGuid());
                bsdoc.Add(DBConstants.EngineSN, serialGenerator());
                
                Temp = bsdoc.Clone();
                lis.Add(((BsonDocument)Temp));
                bsdoc.Remove(DBConstants.Id);
            }


        
        }

        private void createAndInsertData(IMongoCollection<BsonDocument> _collection)
        {
            if (_collection != null)
            {
                try
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        var _documents = new List<BsonDocument>();
                        var _measurement = new BsonDocument();
                        console.AppendText(String.Format("Creating Document: {0}\n", i));
                        var _id = serialGenerator();
                        _measurement.Add(DBConstants.MetaID, _id);
                        _measurement.Add(DBConstants.EngineSN, randomEngineSNGenerator());
                        var _etypeIndex = randomIntegerGenerator(0, 19);
                        var _engineType = sampleTypes[_etypeIndex];
                        _measurement.Add(DBConstants.EngineType, _engineType);
                        _measurement.Add(DBConstants.ModuleSAUSN, serialGenerator());
                        var _mtypeIndex = randomIntegerGenerator(0, 19);
                        var _moduleType = sampleTypes[_mtypeIndex];
                        _measurement.Add(DBConstants.ModuleType, _moduleType);
                        _measurement.Add(DBConstants.LPSSN, serialGenerator());
                        _measurement.Add(DBConstants.StartDateTime, DateTime.Now.AddDays(1.0));
                        _measurement.Add(DBConstants.EndDateTime, DateTime.Now.AddDays(1.0).AddMinutes(12.421));

                        var _temperature = new BsonArray {
                                            new BsonDocument { { DBConstants.ModuleType, _moduleType }, {DBConstants.Sensor, _moduleType }, { DBConstants.Value, randomDoubleGenerator() } },
                                           new BsonDocument { { DBConstants.ModuleType, _engineType }, { DBConstants.Sensor, _engineType }, { DBConstants.Value, randomDoubleGenerator() } }
                                        };

                        _measurement.Add(DBConstants.Temperature, _temperature);
                        var _voltage = new BsonArray {
                                            new BsonDocument { { DBConstants.ModuleType, _moduleType }, { DBConstants.Sensor, _moduleType }, { DBConstants.Value, randomDoubleGenerator() } },
                                           new BsonDocument { { DBConstants.ModuleType, _engineType }, { DBConstants.Sensor, _engineType }, { DBConstants.Value, randomDoubleGenerator() } }
                                        };
                        _measurement.Add(DBConstants.Voltage, _voltage);
                        _measurement.Add(DBConstants.EngineSpearFWVer, versionGenerator());
                        _measurement.Add(DBConstants.EngineLPSFWVer, versionGenerator());
                        _measurement.Add(DBConstants.EngineGDGWVer, versionGenerator());
                        _measurement.Add(DBConstants.ModuleSAUFWVer, versionGenerator());

                        var _moduleOtherFWVer = new BsonArray {
                                            new BsonDocument { { DBConstants.Device, "Peltire Control" }, { DBConstants.Version, versionGenerator() } },
                                           new BsonDocument { { DBConstants.Device, "UMM Drive" }, { DBConstants.Version, versionGenerator() } }
                                        };

                        _measurement.Add(DBConstants.ModuleOtherFWVer, _moduleOtherFWVer);
                        _measurement.Add(DBConstants.MingSWVer, versionGenerator());
                        _measurement.Add(DBConstants.TestPassFail, "Pass");
                        _measurement.Add(DBConstants.ConfigurationORComments, "We are testing");
                        _measurement.Add(DBConstants.Operator, "Prajwol");
                        _measurement.Add(DBConstants.NumberOfLeadingSamplesDropped, 15);
                        _measurement.Add(DBConstants.NumberOfSamplesBeforeFlash, 11);
                        _measurement.Add(DBConstants.NumberOfSamplesAfterFlash, 21);
                        _measurement.Add(DBConstants.NumberOfSamplesAccrossFlash, 13);
                        var testType = randomTestTypeGenerator();
                        _measurement.Add(DBConstants.TestType, testType);
                        _measurement.Add(DBConstants.TestMethod, randomTestMethodGenerator());
                        _measurement.Add(DBConstants.SpectralAveragingTime, randomIntegerGenerator(0, 1));

                        var maxRecordLimit = 450000;
                        for (int K = 1; K <= maxRecordLimit; K++)
                        {

                            var _dataValues = new BsonDocument();

                            _dataValues.Add(DBConstants.WaveLength, randomIntegerGenerator(190, 2200));
                            _dataValues.Add(DBConstants.LampPower, randomIntegerGenerator(0, 100));
                            _dataValues.Add(DBConstants.LampPowerState, "ON");

                            var _detectorChannel = new BsonArray();
                            _detectorChannel.Add(new BsonDocument { { DBConstants.Gain, "high" }, { DBConstants.Step, randomDoubleGenerator() * randomIntegerGenerator(0, 65000) }, { DBConstants.Slope, randomIntegerGenerator(0, 65000) } });
                            _detectorChannel.Add(new BsonDocument { { DBConstants.Gain, "high" }, { DBConstants.Step, randomIntegerGenerator(5000, 65000) - randomIntegerGenerator(0, 5000) }, { DBConstants.Slope, randomIntegerGenerator(0, 65000) } });
                            _detectorChannel.Add(new BsonDocument { { DBConstants.Gain, "high" }, { DBConstants.Step, randomIntegerGenerator(0, 65000) }, { DBConstants.Slope, randomIntegerGenerator(0, 65000) } });
                            _detectorChannel.Add(new BsonDocument { { DBConstants.Gain, "low" }, { DBConstants.Step, randomDoubleGenerator() + randomIntegerGenerator(0, 65000) }, { DBConstants.Slope, randomDoubleGenerator() * randomIntegerGenerator(0, 65000) } });

                            _dataValues.Add(DBConstants.DetectorChannel, _detectorChannel);

                            _dataValues.Add(DBConstants.Filter, "Description of FIlter");
                            _dataValues.Add(DBConstants.Bandwidth, randomDoubleGenerator() * randomIntegerGenerator(0, 5));
                            _dataValues.Add(DBConstants.SlitHeight, "Full");

                            _measurement.Add(DBConstants.Data, _dataValues);
                            _measurement.Add(DBConstants.Id, Guid.NewGuid());
                             var tempMeasurement = (BsonDocument)_measurement.Clone();
                            _documents.Add(tempMeasurement);
                            if (_documents.Count.Equals(5000) || K.Equals(maxRecordLimit))
                            {
                                var startTimeInsert = DateTime.Now;
                                var options = new InsertManyOptions { IsOrdered = false };
                                
                                if (K.Equals(maxRecordLimit))
                                {
                                    console.AppendText("Final Insertion Started...\n");
                                }
                                else
                                {
                                    console.AppendText("Insertion Started...\n");
                                }
                                _collection.InsertMany(_documents, options);
                                console.AppendText("Insertion Complete.\n");
                                var endTimeInsert = DateTime.Now;
                                TimeSpan tspan = endTimeInsert.Subtract(startTimeInsert);

                                if (tspan.Minutes.Equals(0) && tspan.Seconds.Equals(0))
                                {
                                    console.AppendText("Time elapsed: " + tspan.Milliseconds + " milliseconds\n");
                                }
                                else if (tspan.Minutes.Equals(0))
                                {
                                    console.AppendText("Time elapsed: " + tspan.Seconds + ":" + tspan.Milliseconds + " seconds\n");

                                }
                                else {
                                    console.AppendText("Time elapsed: " + tspan.Minutes + ":" + tspan.Seconds + ":" + tspan.Milliseconds + " minutes\n");

                                }
                                _documents.Clear();

                            }
                            _measurement.Remove(DBConstants.Id);
                            _measurement.Remove(DBConstants.Data);
                        }
                        
                        /// Do some calculations for result here
                        var _testResult = new BsonArray();
                        switch (testType)
                        {
                            case "Electronic Noise":
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "En @ 260nm" }, { DBConstants.Value, randomIntegerGenerator(0, 20) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "En @ 190nm" }, { DBConstants.Value, randomIntegerGenerator(20, 200) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                break;
                            case "Light Throughput":
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "Lt @ 260nm" }, { DBConstants.Value, randomIntegerGenerator(100, 209) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "Lt @ 190nm" }, { DBConstants.Value, randomIntegerGenerator(50, 520) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });

                                break;
                            case "0 Abs Noise":
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "Abs SD @ 260nm" }, { DBConstants.Value, randomIntegerGenerator(10, 20) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "Abs SD @ 190nm" }, { DBConstants.Value, randomIntegerGenerator(220, 1220) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "Abs SD @ 500nm" }, { DBConstants.Value, randomIntegerGenerator(30, 520) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "Abs SD @ 800nm" }, { DBConstants.Value, randomIntegerGenerator(1000, 20555) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                break;
                            case "Stray Light (Auto)":
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "Sta @ 260nm" }, { DBConstants.Value, randomIntegerGenerator(0, 20) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "Sta @ 190nm" }, { DBConstants.Value, randomIntegerGenerator(20, 200) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "Sta @ 260nm" }, { DBConstants.Value, randomIntegerGenerator(0, 20) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "Sta @ 190nm" }, { DBConstants.Value, randomIntegerGenerator(20, 200) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });

                                break;
                            case "Minimum SBW":
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "MSBW @ 260nm" }, { DBConstants.Value, randomIntegerGenerator(0, 20) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "MSBW @ 190nm" }, { DBConstants.Value, randomIntegerGenerator(20, 200) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "MSBW @ 260nm" }, { DBConstants.Value, randomIntegerGenerator(100, 209) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "LMSBWt @ 190nm" }, { DBConstants.Value, randomIntegerGenerator(50, 520) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });

                                break;
                            case "Wavelength Drive Check (Short)":
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "WDCS @ 260nm" }, { DBConstants.Value, randomIntegerGenerator(100, 209) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "WDCS @ 190nm" }, { DBConstants.Value, randomIntegerGenerator(20, 200) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "WDCS @ 260nm" }, { DBConstants.Value, randomIntegerGenerator(0, 20) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });

                                break;
                            case "Wavelength Drive Check (Long)":
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "WDCl @ 190nm" }, { DBConstants.Value, randomIntegerGenerator(50, 520) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });

                                break;
                            case "Stray Light (Manual)":
                                _testResult.Add(new BsonDocument { { DBConstants.Description, "SL @ 190nm" }, { DBConstants.Value, randomIntegerGenerator(500, 520) }, { DBConstants.Criteria, "Has to meet certain criteria" }, { DBConstants.PassFail, 1 } });
                                break;

                        }

                        var builder = Builders<BsonDocument>.Filter;
                        var filter = builder.Eq(DBConstants.MetaID, _id);
                        console.AppendText("Update Documents Started...\n");
                        var startTime = DateTime.Now;
                        var _result = _collection.UpdateMany(filter, Builders<BsonDocument>.Update.Set(DBConstants.TestResult, _testResult));
                        if (_result.IsModifiedCountAvailable && _result.IsAcknowledged)
                        {
                            console.AppendText(String.Format("{0} records updated.\n", _result.ModifiedCount));
                        }
                        var endTime = DateTime.Now;
                        TimeSpan ts = endTime.Subtract(startTime);

                        if (ts.Minutes.Equals(0) && ts.Seconds.Equals(0))
                        {
                            console.AppendText("Time elapsed: " + ts.Milliseconds + " milliseconds\n");
                        }
                        else if (ts.Minutes.Equals(0))
                        {
                            console.AppendText("Time elapsed: " + ts.Seconds + ":" + ts.Milliseconds + " seconds\n");

                        }
                        else {
                            console.AppendText("Time elapsed: " + ts.Minutes + ":" + ts.Seconds + ":" + ts.Milliseconds + " minutes\n");

                        }
                        console.AppendText(String.Format("Automated Test '{0}' Complete.\n", testType));
                    }
                    console.AppendText("Data Generation Completed.\n");
                    console.AppendText("==================================================\n");
                }
                catch (Exception ex)
                {
                    console.AppendText("Data Generation Failed.\n");
                    console.AppendText("==================================================\n");
                    console.AppendText(String.Format("[Error]: {0}", ex.Message));

                    //throw;
                }
            }
        }

        private String randomTestTypeGenerator()
        {
            return testTypes[randomIntegerGenerator(0, 8)];
        }

        private String randomEngineSNGenerator()
        {
            return instruments[randomIntegerGenerator(0, 5)];
        }

        private String randomTestMethodGenerator()
        {
            return testMethods[randomIntegerGenerator(0, 2)];
        }

        private String serialGenerator()
        {
            return Guid.NewGuid().ToString();
        }

        private float randomDoubleGenerator()
        {
            return (float)new Random().NextDouble();
        }

        private String versionGenerator()
        {
            return randomDoubleGenerator().ToString().Substring(0, 6);
        }

        private int randomIntegerGenerator(int min, int max)
        {
            return new Random().Next(min, max);
        }
    }
} 