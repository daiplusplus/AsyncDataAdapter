using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using Shouldly;

namespace AsyncDataAdapter.Tests
{
    /// <summary>Just some meta-tests so I know my FakeDb stuff works.</summary>
    public class RandomDataTests
    {
        [Test]
        public void FakeDbDataReader_Sync_should_behave()
        {
            FakeDbCommand cmd = new FakeDbCommand();

            FakeDbDataReader rdr = new FakeDbDataReader( cmd );

            //

            List<TestTable> tables = RandomDataGenerator.CreateRandomTables( seed: 1234, tableCount: 5 );

            rdr.ResetAndLoadTestData( tables );

            rdr.AllTables.Count.ShouldBe( 5 );
            rdr.AllTables[0].Rows.Count.ShouldBe( 40 );
            rdr.AllTables[1].Rows.Count.ShouldBe( 52 );
            rdr.AllTables[2].Rows.Count.ShouldBe( 79 );
            rdr.AllTables[3].Rows.Count.ShouldBe( 37 );
            rdr.AllTables[4].Rows.Count.ShouldBe( 31 );

            //

            rdr.AsyncMode = AsyncMode.AllowSync;

            // Table 0:
            {
                Int32 i = 0;
                while( rdr.Read() )
                {
                    i++;
                }

                i.ShouldBe( tables[0].Rows.Count );
            }

            // Table 1:
            rdr.NextResult().ShouldBeTrue();
            {
                Int32 i = 0;
                while( rdr.Read() )
                {
                    i++;
                }

                i.ShouldBe( tables[1].Rows.Count );
            }

            // Table 2:
            rdr.NextResult().ShouldBeTrue();
            {
                Int32 i = 0;
                while( rdr.Read() )
                {
                    i++;
                }

                i.ShouldBe( tables[2].Rows.Count );
            }

            // Table 3:
            rdr.NextResult().ShouldBeTrue();
            {
                Int32 i = 0;
                while( rdr.Read() )
                {
                    i++;
                }

                i.ShouldBe( tables[3].Rows.Count );
            }

            // Table 4:
            rdr.NextResult().ShouldBeTrue();
            {
                Int32 i = 0;
                while( rdr.Read() )
                {
                    i++;
                }

                i.ShouldBe( tables[4].Rows.Count );
            }

            rdr.NextResult().ShouldBeFalse();
        }

        [Test]
        public async Task FakeDbDataReader_Async_should_behave()
        {
            FakeDbCommand cmd = new FakeDbCommand();

            FakeDbDataReader rdr = new FakeDbDataReader( cmd );

            //

            List<TestTable> tables = RandomDataGenerator.CreateRandomTables( seed: 1234, tableCount: 5 );

            rdr.ResetAndLoadTestData( tables );

            rdr.AllTables.Count.ShouldBe( 5 );

            //

            rdr.AsyncMode = AsyncMode.AwaitAsync;

            // Table 0:
            {
                Int32 i = 0;
                while( await rdr.ReadAsync() )
                {
                    i++;
                }

                i.ShouldBe( tables[0].Rows.Count );
            }

            // Table 1:
            ( await rdr.NextResultAsync() ).ShouldBeTrue();
            {
                Int32 i = 0;
                while( await rdr.ReadAsync() )
                {
                    i++;
                }

                i.ShouldBe( tables[1].Rows.Count );
            }

            // Table 2:
            ( await rdr.NextResultAsync() ).ShouldBeTrue();
            {
                Int32 i = 0;
                while( await rdr.ReadAsync() )
                {
                    i++;
                }

                i.ShouldBe( tables[2].Rows.Count );
            }

            // Table 3:
            ( await rdr.NextResultAsync() ).ShouldBeTrue();
            {
                Int32 i = 0;
                while( await rdr.ReadAsync() )
                {
                    i++;
                }

                i.ShouldBe( tables[3].Rows.Count );
            }

            // Table 4:
            ( await rdr.NextResultAsync() ).ShouldBeTrue();
            {
                Int32 i = 0;
                while( await rdr.ReadAsync() )
                {
                    i++;
                }

                i.ShouldBe( tables[4].Rows.Count );
            }

            ( await rdr.NextResultAsync() ).ShouldBeFalse();
        }
    }
}